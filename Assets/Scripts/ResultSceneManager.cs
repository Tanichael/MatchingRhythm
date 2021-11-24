using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] Text m_TitleText;
    [SerializeField] Text m_ScoreText;
    [SerializeField] Text m_ComboText;
    [SerializeField] Text[] m_CountTexts;
    [SerializeField] Button m_MusicSelectButton;

    private void OnEnable()
    {
        m_MusicSelectButton.interactable = true;
        m_MusicSelectButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SceneLoader.Instance.GoSceneAsync("MusicSelectScene").Forget();
            });
           

        string title = DataManager.Instance.MusicData.Title;
        float score = DataManager.Instance.Score;
        int maxCombo = DataManager.Instance.MaxCombo;
        Dictionary<HitResult.ResultState, int> countDictionary = DataManager.Instance.CountDictionary;

        HitResult[] hitResults = HitResultMasterData.Instance.HitResults;

        m_TitleText.text = title;
        m_ScoreText.text = score.ToString();
        m_ComboText.text = maxCombo.ToString();

        foreach(var hitResult in hitResults)
        {
            string tempText;
            if(countDictionary.ContainsKey(hitResult.State))
            {
                tempText = countDictionary[hitResult.State].ToString();
            }
            else
            {
                tempText = "0";
            }
            m_CountTexts[(int)hitResult.State].text = tempText;
        }
    }
}
