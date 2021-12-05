﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Cysharp.Threading.Tasks;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject m_LoadingCover;
    [SerializeField] private GameObject m_TransparentCover;
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioSource m_IsSelectedAudio;

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

        if (scene == "GameScene")
        {
            m_IsSelectedAudio.Play();
        }

        m_LoadingCover.SetActive(true);
        //now loading ... の点の数を変える処理
        await SceneManager.LoadSceneAsync(scene);
        Observable.Timer(TimeSpan.FromMilliseconds(200))
                    .Subscribe(__ =>
                    {
                        m_LoadingCover.SetActive(false);
                    });
    }

    public void TransparentCover()
    {
        m_TransparentCover.SetActive(false);
        m_TransparentCover.SetActive(true);
    }

    public void TransparentUnCover()
    {
        m_TransparentCover.SetActive(true);
        m_TransparentCover.SetActive(false);
    }
}
