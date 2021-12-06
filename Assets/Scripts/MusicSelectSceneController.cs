using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelectSceneController : MonoBehaviour
{
    [SerializeField] private ScrollRect m_MusicSelectScrollRect;
    [SerializeField] private GameObject m_Content;
    [SerializeField] private MusicElement m_MusicElement;
    [SerializeField] private AudioSource m_MusicSelectAudio;

    private List<MusicElement> m_MusicElements;

    private readonly float ms_HorizontalRange = 320f;
    private readonly float ms_VerticalRange = 380f;
    private readonly float ms_MarginHeight = 250f;

    private MusicData[] m_MusicDataList;

    private void OnEnable()
    {
        m_MusicElements = new List<MusicElement>();
        m_MusicDataList = MusicMasterData.Instance.MusicDataList;
        MakeMusicElements(m_MusicDataList);
        m_MusicSelectAudio.Play();
    }

    private void MakeMusicElements(MusicData[] musicDataList)
    {
        RectTransform contentRectTransform = m_Content.transform as RectTransform;
        if(contentRectTransform != null)
        {
            int size = musicDataList.Length;
            int column = (size - 1) / 3;
            float height = ms_MarginHeight * 2f + column * ms_VerticalRange;
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, height);
        }

        for(int index = 0; index < musicDataList.Length; index++)
        {
            foreach (var musicData in musicDataList)
            {
                if (musicData.Id == index)
                {
                    string titleText = musicData.Title.Replace(" ", "\n");
                    if (index == 0)
                    {
                        m_MusicElements.Add(m_MusicElement);
                        index++;
                        m_MusicElement.MusicData = musicData;

                        m_MusicElement.TitleText.text = titleText;
                        continue;
                    }
                    MusicElement musicElement = Instantiate(m_MusicElement, m_MusicSelectScrollRect.content, true);
                    RectTransform rectTransform = musicElement.transform as RectTransform;

                    int right = index % 3;
                    int down = index / 3;

                    Debug.Log("down = " + down);
                    Debug.Log("right = " + right);

                    rectTransform.anchoredPosition += new Vector2(ms_HorizontalRange * right, -ms_VerticalRange * down);

                    //MusicDataの設定
                    musicElement.MusicData = musicData;
                    musicElement.TitleText.text = titleText;

                    //リストに加える
                    m_MusicElements.Add(musicElement);
                }
            }
        }
        
    }

}

