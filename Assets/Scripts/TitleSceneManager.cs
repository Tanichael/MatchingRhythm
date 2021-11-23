using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] Button m_StartButton;

    private void OnEnable()
    {
        m_StartButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SceneManager.LoadScene("MusicSelectScene");
                m_StartButton.interactable = false;
            });
    }
}
