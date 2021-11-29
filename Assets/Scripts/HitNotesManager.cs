using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ノーツを叩いた時の処理をまとめたクラス
/// スコア関連のデータ更新についてはScoreControllerクラスに処理を委譲する
/// </summary>
public class HitNotesManager : MonoBehaviour
{
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private ScoreController m_ScoreController;
    [SerializeField] private SpriteRenderer m_ResultSpriteRenderer;

    private HitResult[] m_HitResults;

    private void OnEnable()
    {
        m_HitResults = HitResultMasterData.Instance.HitResults;

        m_GameManager
            .OnHitNotes
            .Subscribe(resultState =>
            {
                OnHitNotes(resultState);
            });

    }

    //判定に応じて処理をする関数
    private void OnHitNotes(HitResult.ResultState resultState)
    {
        //resultStateをもとに判定がどれか探す
        foreach(var hitResult in m_HitResults)
        {
            if(hitResult.State == resultState)
            {
                //判定結果のエフェクトを出す
                //ここはオブジェクトは常駐させておいてspriteを変更する方が良くない？
                m_ResultSpriteRenderer.sprite = hitResult.ResultSprite;
                m_ResultSpriteRenderer.gameObject.SetActive(false);
                m_ResultSpriteRenderer.gameObject.SetActive(true);

                Observable.Timer(TimeSpan.FromMilliseconds(200))
                    .Subscribe(_ => m_ResultSpriteRenderer.gameObject.SetActive(false));

                m_ScoreController.CalculateScore(hitResult);
            }
        }

    }
}
