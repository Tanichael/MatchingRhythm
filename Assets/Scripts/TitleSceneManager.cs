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
    [SerializeField] SceneLoader m_SceneLoader;
    [SerializeField] AudioSource m_TitleAudio;

    private bool m_IsClick;
    private bool m_LoadStart;

    private void OnEnable()
    {
        m_IsClick = false;
        m_LoadStart = false;

        //以下本番用
        this.UpdateAsObservable()
            .Where(_ => m_IsClick == false)
            .Where(_ => Input.GetMouseButtonDown(0) == true)
            .Subscribe(_ =>
            {
                m_TitleAudio.Play();
                m_IsClick = true;
            });
        this.UpdateAsObservable()
            .Where(_ =>
            {
                return (m_IsClick == true && m_TitleAudio.isPlaying == false && m_LoadStart == false);
            })
            .Subscribe(_ =>
            {
                m_LoadStart = true;
                m_SceneLoader.GoSceneAsync("MusicSelectScene").Forget();
            });
    }
}
