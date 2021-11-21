using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class MusicElement : MonoBehaviour
{
    [SerializeField] private Button m_MusicButton;
    [SerializeField] private Text m_TitleText;

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

    private void OnEnable()
    {
        m_MusicButton.onClick
            .AsObservable()
            .Subscribe(_ =>
            {
                //拡大みたいなの挟んだ方がよさそう、とりあえず暫定
                DataManager.Instance.MusicData = m_MusicData;
                //シーン切り替え 読み込みどこ？
                //シーン切り替えた後ロードして準備ができたら音楽を流す、って感じで良さそう
                //つまりここではとりあえずシーンを切り替える
                SceneManager.LoadScene("GameScene");
            });
    }

    
}
