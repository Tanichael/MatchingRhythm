using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class MusicElement : MonoBehaviour
{
    [SerializeField] private Button m_MusicButton;
    [SerializeField] private Text m_TitleText;
    [SerializeField] private MusicSelectSceneController m_MusicSelectSceneController;
    [SerializeField] private PlayGroupController m_PlayGroupController;

    private MusicData m_MusicData;

    public Text TitleText
    {
        get
        {
            return m_TitleText;
        }
        set
        {
            m_TitleText = value;
        }
    }

    public MusicData MusicData
    {
        get
        {
            return m_MusicData;
        }
        set
        {
            m_MusicData = value;
        }
    }

    public MusicSelectSceneController MusicSelectSceneController
    {
        get
        {
            return m_MusicSelectSceneController;
        }
    }

    private void OnEnable()
    {
        m_MusicButton.onClick
            .AsObservable()
            .Subscribe(_ =>
            {
                SceneLoader.Instance.TransparentCover();
                m_PlayGroupController.SetUp(m_MusicData);
            });

    }

    
}
