using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class PlayGroupController : MonoBehaviour
{
    [SerializeField] private Button m_PlayButton;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private Text m_TitleText;
    [SerializeField] private Text m_HighMoteIndexText;
    [SerializeField] private Text m_HighComboText;
    [SerializeField] private MusicElement m_MusicElement;

    private readonly float ms_HorizontalRange = 320f;
    private readonly float ms_VerticalRange = 380f;
    private readonly float ms_CheckDist = 0.0001f;

    private bool m_IsBack = false;
    private bool m_IsPlay = false;
    private int m_Id;

    private void OnEnable()
    {
        RectTransform rectTransform = this.gameObject.transform as RectTransform;

        m_PlayButton.interactable = false;
        m_BackButton.interactable = false;

        m_IsBack = false;
        m_IsPlay = false;

        this.UpdateAsObservable()
            .Where(_ => m_IsBack == false)
            .Subscribe(__ =>
            {
                Vector2 finalAnchorMin = new Vector2(0f, 0f);
                Vector2 finalAnchorMax = new Vector2(1f, 1f);
                Vector2 finalPosition = new Vector2(0f, 0f);

                if ((Vector2.Distance(rectTransform.anchorMin, finalAnchorMin) > ms_CheckDist) || (Vector2.Distance(rectTransform.anchorMax, finalAnchorMax) > ms_CheckDist) || (Vector2.Distance(rectTransform.anchoredPosition, finalPosition) > ms_CheckDist))
                {
                    rectTransform.anchorMin = Vector2.Lerp(rectTransform.anchorMin, finalAnchorMin, 0.3f);
                    rectTransform.anchorMax = Vector2.Lerp(rectTransform.anchorMax, finalAnchorMax, 0.3f);
                    rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, finalPosition, 0.3f);
                }

                if ((Vector2.Distance(rectTransform.anchorMin, finalAnchorMin) <= ms_CheckDist) && (Vector2.Distance(rectTransform.anchorMax, finalAnchorMax) <= ms_CheckDist) && (Vector2.Distance(rectTransform.anchoredPosition, finalPosition) <= ms_CheckDist))
                {
                    m_PlayButton.interactable = true;
                    m_BackButton.interactable = true;
                    SceneLoader.Instance.TransparentUnCover();
                }

            });


        this.UpdateAsObservable()
            .Where(_ => m_IsBack == true)
            .Subscribe(__ =>
            {
                m_PlayButton.interactable = false;
                m_BackButton.interactable = false;
                m_IsBack = false;

                this.gameObject.SetActive(false);
                SceneLoader.Instance.TransparentUnCover();

                //Vector2 finalAnchorMin = new Vector2(0.5f, 1f);
                //Vector2 finalAnchorMax = new Vector2(0.5f, 1f);

                //RectTransform baseRectTransform = m_MusicElement.transform as RectTransform;
                //int right = m_Id % 3;
                //int down = m_Id / 3;
                //Vector2 finalPosition = baseRectTransform.anchoredPosition + new Vector2(ms_HorizontalRange * right, -ms_VerticalRange * down);

                //if ((Vector2.Distance(rectTransform.anchorMin, finalAnchorMin) > ms_CheckDist) || (Vector2.Distance(rectTransform.anchorMax, finalAnchorMax) > ms_CheckDist) || (Vector2.Distance(rectTransform.anchoredPosition, finalPosition) > ms_CheckDist))
                //{
                //    rectTransform.anchorMin = Vector2.Lerp(rectTransform.anchorMin, finalAnchorMin, 0.3f);
                //    rectTransform.anchorMax = Vector2.Lerp(rectTransform.anchorMax, finalAnchorMax, 0.3f);
                //    rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, finalPosition, 0.3f);
                //}

                //if ((Vector2.Distance(rectTransform.anchorMin, finalAnchorMin) <= ms_CheckDist) && (Vector2.Distance(rectTransform.anchorMax, finalAnchorMax) <= ms_CheckDist))
                //{
                //    this.gameObject.SetActive(false);
                //    m_IsBack = false;
                //    SceneLoader.Instance.TransparentUnCover();
                //}

            });

    }

    public void SetUp(MusicData musicData)
    {
        //位置、サイズの設定
        RectTransform rectTransform = this.gameObject.transform as RectTransform;
        int id = musicData.Id;
        m_Id = id;
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);

            RectTransform baseRectTransform = m_MusicElement.transform as RectTransform;

            int right = id % 3;
            int down = id / 3;

            rectTransform.anchoredPosition = baseRectTransform.anchoredPosition + new Vector2(ms_HorizontalRange * right, -ms_VerticalRange * down);
        }
        m_IsBack = false;

        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);

        //各種テキストの設定
        string title = musicData.Title.Replace(" ", "");
        float highMoteIndex = UserData.Instance.GetHighMoteIndex(musicData.Id);
        int highCombo = UserData.Instance.GetHighCombo(musicData.Id);
        string highMoteIndexText = highMoteIndex.ToString() + "％";
        string highComboText = highCombo.ToString();

        m_TitleText.text = title;
        m_HighMoteIndexText.text = highMoteIndexText;
        m_HighComboText.text = highComboText;

        m_PlayButton.onClick
           .AsObservable()
           .Where(_ => m_IsPlay == false)
           .Subscribe(__ =>
           {
                m_IsPlay = true;
                //拡大みたいなの挟んだ方がよさそう、とりあえず暫定
                DataManager.Instance.MusicData = musicData;
                //シーン切り替え 読み込みどこ？
                //シーン切り替えた後ロードして準備ができたら音楽を流す、って感じで良さそう
                //つまりここではとりあえずシーンを切り替える
                SceneLoader.Instance.GoSceneAsync("GameScene").Forget();
           });

        m_BackButton.onClick
            .AsObservable()
            .Subscribe(_ =>
            {
                SceneLoader.Instance.TransparentCover();
                m_IsBack = true;
            });
    }
}
