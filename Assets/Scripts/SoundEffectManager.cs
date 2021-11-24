using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] GameManager m_GameManager;
    [SerializeField] AudioSource m_BeautifulPlayer;
    [SerializeField] AudioSource m_UglyPlayer;

    private void OnEnable()
    {
        m_GameManager
            .OnSoundEffect
            .Where(type => type == "beautiful")
            .Subscribe(type => BeautifulPlay());

        m_GameManager
            .OnSoundEffect
            .Where(type => type == "ugly")
            .Subscribe(type => UglyPlay());
    }

    private void BeautifulPlay()
    {
        m_BeautifulPlayer.Stop();
        m_BeautifulPlayer.Play();
    }

    private void UglyPlay()
    {
        m_UglyPlayer.Stop();
        m_UglyPlayer.Play();
    }
}
