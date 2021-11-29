using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコア関連データ更新クラス
/// </summary>
public class ScoreController : MonoBehaviour
{
    [SerializeField] private GameObject m_ComboGroup;
    [SerializeField] private Text m_ComboText;
    [SerializeField] private ImgsFillDynamic m_ImgsFillDynamic;

    /// <summary>
    /// HitResultを引数にしてスコア関連のデータを更新する関数
    /// </summary>
    /// 
    /// <param name="hitResult">hitResult</param>
    /// <returns>現在のスコア</returns>
    public void CalculateScore(HitResult hitResult)
    {
        //DataManagerから必要なデータをロードする
        float score = DataManager.Instance.Score;
        int combo = DataManager.Instance.Combo;
        int resultCount;
        int maxCombo = DataManager.Instance.MaxCombo;
        int notesCount = DataManager.Instance.NotesCount;
        float fullScore = DataManager.Instance.FullScore;

        if (DataManager.Instance.CountDictionary.ContainsKey(hitResult.State) == false)
        {
            DataManager.Instance.CountDictionary.Add(hitResult.State, 0);
            resultCount = 0;
        } else {
            resultCount = DataManager.Instance.CountDictionary[hitResult.State];
        }

        //コンボ数の処理 IsCombo == true ならコンボが続く
        if(hitResult.IsCombo == true)
        {
            combo += 1;
            DataManager.Instance.Combo = combo;
            if (maxCombo < combo)
            {
                DataManager.Instance.MaxCombo = combo;
            }
        }

        //コンボが終わった時
        if(hitResult.IsCombo == false)
        {
            combo = 0;
            DataManager.Instance.Combo = combo;
        }

        //判定の数を更新
        resultCount = resultCount + 1;
        DataManager.Instance.CountDictionary[hitResult.State] = resultCount;

        //スコアの計算
        float ratio = (float)(combo / notesCount); //全ノーツ数中の割合

        float baseScore = HitResultMasterData.Instance.BaseScore;
        float scoreRate = hitResult.ScoreRate;
        float comboRate = ComboRateMasterData.Instance.GetComboRate(ratio);
        score += baseScore * scoreRate * comboRate;

        //スコアの更新
        DataManager.Instance.Score = score;

        //UI変更の処理
        if (combo == 0)
        {
            m_ComboGroup.SetActive(false);
        } else
        {
            m_ComboGroup.SetActive(true);

            string comboText = combo.ToString();
            m_ComboText.text = comboText;
        }

        float scoreRatio = score / fullScore;
        m_ImgsFillDynamic.SetValue(scoreRatio, true);

    }
}
