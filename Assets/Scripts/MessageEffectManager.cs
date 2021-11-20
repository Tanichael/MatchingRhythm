using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MessageEffectManager : MonoBehaviour
{
    [SerializeField] GameManager m_GameManager;
    [SerializeField] GameObject m_Good;
    [SerializeField] GameObject m_Failure;

    private void OnEnable()
    {
        m_GameManager
            .OnMessageEffect
            .Where(result => result == "good")
            .Subscribe(result => ShowGood());

        m_GameManager
            .OnMessageEffect
            .Where(result => result == "failure")
            .Subscribe(result => ShowFailure());
    }

    private void ShowGood()
    {
        m_Good.SetActive(false);
        m_Good.SetActive(true);

        Observable.Timer(TimeSpan.FromMilliseconds(200))
            .Subscribe(_ => m_Good.SetActive(false));
    }

    private void ShowFailure()
    {
        m_Failure.SetActive(false);
        m_Failure.SetActive(true);

        Observable.Timer(TimeSpan.FromMilliseconds(200))
            .Subscribe(_ => m_Failure.SetActive(false));
    }
}
