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
    [SerializeField] Text m_MoteText;
    [SerializeField] Text m_ComboText;
    [SerializeField] Text[] m_CountTexts;
    [SerializeField] AudioSource m_ResultAudio;
    [SerializeField] Button[] m_Buttons;
    [SerializeField] Button m_MusicSelectButton;
    [SerializeField] FullComboEffect m_FullComboEffect;

    private readonly float ms_CheckDist = 0.1f;
    private readonly float ms_FinalX = 27f;
    private readonly float ms_StartRange = 200f;

    private float m_StartTime;

    private void OnEnable()
    {
        m_StartTime = Time.time * 1000;

        m_MusicSelectButton.interactable = true;
        m_MusicSelectButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SceneLoader.Instance.GoSceneAsync("MusicSelectScene").Forget();
            });

        this.UpdateAsObservable()
               .Subscribe(_ =>
               {
                   int cnt = 0;
                   foreach(var button in m_Buttons)
                   {
                       RectTransform rectTransform = button.transform as RectTransform;
                       Vector2 finalPosition = new Vector2(ms_FinalX, rectTransform.anchoredPosition.y);
                       if (Vector2.Distance(rectTransform.position, finalPosition) > ms_CheckDist)
                       {
                           if(Time.time * 1000 - m_StartTime > (cnt+1) * ms_StartRange)
                           {
                                MoveButton(button, finalPosition);
                           }
                       }
                       cnt = cnt + 1;
                   }
               });

        m_ResultAudio.Play();

        string title = DataManager.Instance.MusicData.Title;
        float score = DataManager.Instance.Score;
        int maxCombo = DataManager.Instance.MaxCombo;
        float fullScore = DataManager.Instance.FullScore;
        int notesCount = DataManager.Instance.NotesCount;
        int id = DataManager.Instance.MusicData.Id;
        Dictionary<HitResult.ResultState, int> countDictionary = DataManager.Instance.CountDictionary;

        HitResult[] hitResults = HitResultMasterData.Instance.HitResults;

        float moteIndex = Mathf.Round(score / fullScore * 100);

        title = title.Replace(" ", "");

        //フルコンボかどうかの判定
        if(notesCount == maxCombo)
        {
            m_FullComboEffect.gameObject.SetActive(true);
            m_FullComboEffect.FullComboAudio.Stop();
            m_FullComboEffect.FullComboAudio.Play();
        }

        m_TitleText.text = title;
        m_MoteText.text = moteIndex.ToString() + "％";
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

        //UserData更新処理
        float highMoteIndex = UserData.Instance.GetHighMoteIndex(id);
        int highCombo = UserData.Instance.GetHighCombo(id);

        if(moteIndex > highMoteIndex)
        {
            UserData.Instance.SetHighMoteIndex(id, moteIndex);
        }

        if (maxCombo > highCombo)
        {
            UserData.Instance.SetHighCombo(id, maxCombo);
        }

    }

    private void MoveButton(Button button, Vector2 finalPosition)
    {
        RectTransform currentRectTransform = (RectTransform)button.transform;
        currentRectTransform.anchoredPosition = Vector2.Lerp(currentRectTransform.anchoredPosition, finalPosition, 0.1f);
    }
}
