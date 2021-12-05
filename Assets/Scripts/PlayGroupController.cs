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
    private readonly float ms_CheckDist = 0.1f;

    public void SetUp(MusicData musicData)
    {
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

        //位置、サイズの設定
        RectTransform rectTransform = this.gameObject.transform as RectTransform;
        int id = musicData.Id;
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);

            RectTransform baseRectTransform = m_MusicElement.transform as RectTransform;

            int right = id % 3;
            int down = id / 3;

            rectTransform.anchoredPosition = baseRectTransform.anchoredPosition + new Vector2(ms_HorizontalRange * right, -ms_VerticalRange * down);
        }

        m_PlayButton.interactable = false;
        m_BackButton.interactable = false;

        this.UpdateAsObservable()
            .Subscribe(_ =>
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

        m_PlayButton.onClick
            .AsObservable()
            .Subscribe(_ =>
            {
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
                this.gameObject.SetActive(false);
                if (rectTransform != null)
                {
                    rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    rectTransform.anchorMax = new Vector2(0.5f, 1f);

                    RectTransform baseRectTransform = m_MusicElement.transform as RectTransform;

                    int right = id % 3;
                    int down = id / 3;

                    rectTransform.anchoredPosition = baseRectTransform.anchoredPosition + new Vector2(ms_HorizontalRange * right, -ms_VerticalRange * down);
                }

            });


    }
}
