using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelectSceneController : MonoBehaviour
{
    [SerializeField] private ScrollRect m_MusicSelectScrollRect;
    [SerializeField] private MusicElement m_MusicElement;
    [SerializeField] private AudioSource m_MusicSelectAudio;

    private List<MusicElement> m_MusicElements = new List<MusicElement>();

    private readonly float ms_HorizontalRange = 320f;
    private readonly float ms_VerticalRange = 380f;

    private MusicData[] m_MusicDataList;

    private void OnEnable()
    {
        //m_MusicDataList = Resources.Load<MusicMasterData>("MusicMasterData").MusicDataList;
        m_MusicDataList = MusicMasterData.Instance.MusicDataList;
        MakeMusicElements(m_MusicDataList);
        m_MusicSelectAudio.Play();
    }

    private void MakeMusicElements(MusicData[] musicDataList)
    {
        int index = 0;

        foreach(var musicData in musicDataList)
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

            index++;
        }
    }

}

