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
                //拡大みたいなの挟んだ方がよさそう、とりあえず暫定
                DataManager.Instance.MusicData = m_MusicData;
                //シーン切り替え 読み込みどこ？
                //シーン切り替えた後ロードして準備ができたら音楽を流す、って感じで良さそう
                //つまりここではとりあえずシーンを切り替える
                SceneLoader.Instance.GoSceneAsync("GameScene").Forget();

            });

    }

    
}
