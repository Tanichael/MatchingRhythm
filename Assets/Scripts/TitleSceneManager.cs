using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] Button m_StartButton;
    [SerializeField] SceneLoader m_SceneLoader;
    [SerializeField] Button m_GoResultButton;

    private void OnEnable()
    {
        m_StartButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                m_SceneLoader.GoSceneAsync("MusicSelectScene").Forget();
            });

        m_GoResultButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                m_SceneLoader.GoSceneAsync("ResultScene").Forget();
            });
    }
}
