using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア関連データ更新クラス
/// </summary>
public class ScoreController : MonoBehaviour
{
    /// <summary>
    /// HitResultを引数にしてスコア関連のデータを更新するクラス
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

        if (DataManager.Instance.CountDictionary.ContainsKey(hitResult.Result) == false)
        {
            DataManager.Instance.CountDictionary.Add(hitResult.Result, 0);
            resultCount = 0;
        } else {
            resultCount = DataManager.Instance.CountDictionary[hitResult.Result];
        }

        //ベーススコアをマスターデータからロード
        float baseScore = HitResultMasterData.Instance.BaseScore;

        //スコアの計算
        float resultRate = CalculateResultRate(hitResult);
        float comboRate = CalculateComboRate(hitResult);
        score += baseScore * hitResult.ScoreRate * comboRate;

        //スコアの更新
        DataManager.Instance.Score = score;

        //コンボ数の処理 暫定はgoodの時のみカウント
        if(hitResult.Result == "good")
        {
            combo += 1;
            DataManager.Instance.Combo = combo;
        }

        if(hitResult.Result == "failure")
        {
            combo = 0;
            DataManager.Instance.Combo = combo;
        }

        //判定の数を更新
        resultCount = resultCount + 1;
        DataManager.Instance.CountDictionary[hitResult.Result] = resultCount;

        Debug.Log("Score = " + score);
        Debug.Log("Combo = " + combo);
    }

    /// <summary>
    /// 判定倍率を計算する関数
    /// </summary>
    /// <param name="hitResult"></param>
    /// <returns>判定倍率</returns>
    private float CalculateResultRate(HitResult hitResult)
    {
        float resultRate = 1.0f;
        return resultRate;
    }

    /// <summary>
    /// コンボ倍率を計算する関数
    /// </summary>
    /// <param name="hitResult"></param>
    /// <returns>コンボ倍率</returns>
    private float CalculateComboRate(HitResult hitResult)
    {
        float comboRate = 1f;
        //全ノーツ数はDataManagerのMusicDataから計算する
        return comboRate;
    }
}
