using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelectSceneController : MonoBehaviour
{
    [SerializeField] private ScrollRect m_MusicSelectScrollRect;
    [SerializeField] private MusicElement m_MusicElement;
    [SerializeField] private Button m_Cover;

    private List<MusicElement> m_MusicElements = new List<MusicElement>();

    private readonly float ms_HorizontalRange = 280f;
    private readonly float ms_VerticalRange = 330f;

    private MusicData[] m_MusicDataList;

    public Button Cover
    {
        get
        {
            return m_Cover;
        }
    }

    private void OnEnable()
    {
        m_Cover.gameObject.SetActive(false);
        m_Cover.interactable = false;
        //m_MusicDataList = Resources.Load<MusicMasterData>("MusicMasterData").MusicDataList;
        m_MusicDataList = MusicMasterData.Instance.MusicDataList;
        MakeMusicElements(m_MusicDataList);
    }

    private void MakeMusicElements(MusicData[] musicDataList)
    {
        int index = 0;

        foreach(var musicData in musicDataList)
        {
            Debug.Log(index);
            if(index == 0)
            {
                m_MusicElements.Add(m_MusicElement);
                index++;
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
            musicElement.TitleText.text = musicData.Title;

            //リストに加える
            m_MusicElements.Add(musicElement);

            index++;
        }
    }

}

