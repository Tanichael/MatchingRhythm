using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Cysharp.Threading.Tasks;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject m_LoadingCover;
    [SerializeField] private AudioSource m_AudioSource;

    private static SceneLoader m_Instance;

    public static SceneLoader Instance
    {
        get
        {
            return m_Instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        m_LoadingCover.SetActive(false);
        m_Instance = this;
    }

    public async UniTask GoSceneAsync(string scene)
    {
        if(scene == "MusicSelectScene")
        {
            m_AudioSource.Play();
        }
        m_LoadingCover.SetActive(true);
        //now loading ... の点の数を変える処理
        await SceneManager.LoadSceneAsync(scene);
        m_LoadingCover.SetActive(false);
    }
}
