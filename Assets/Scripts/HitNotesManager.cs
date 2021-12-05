﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ノーツを叩いた時の処理をまとめたクラス
/// スコア関連のデータ更新についてはScoreControllerクラスに処理を委譲する
/// </summary>
public class HitNotesManager : MonoBehaviour
{
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private ScoreController m_ScoreController;
    [SerializeField] RectTransform m_ResultTextPoint;

    private HitResult[] m_HitResults;
    private Dictionary<HitResult, GameObject> m_ResultTextDictionary;

    private void OnEnable()
    {
        m_HitResults = HitResultMasterData.Instance.HitResults;
        m_ResultTextDictionary = new Dictionary<HitResult, GameObject>();

        foreach (var hitResult in m_HitResults)
        {
            GameObject resultTextObject = Instantiate(hitResult.ResultText, m_ResultTextPoint);
            resultTextObject.SetActive(false);
            if (m_ResultTextDictionary.ContainsKey(hitResult) == false)
            {
                m_ResultTextDictionary.Add(hitResult, resultTextObject);
            }
            else
            {
                m_ResultTextDictionary[hitResult] = resultTextObject;
            }
        }

        m_GameManager
            .OnHitNotes
            .Subscribe(resultState =>
            {
                OnHitNotes(resultState);
            });

    }

    //判定に応じて処理をする関数
    //resultStateを渡すよりresultStateでhitResultインスタンス作った方がいい？
    //HitResultはモデルとなるクラスだからインスタンス化することはしない。セットが決まってるから。
    private void OnHitNotes(HitResult.ResultState resultState)
    {
        //resultStateをもとに判定がどれか探す
        foreach (var hitResult in m_HitResults)
        {
            if (hitResult.State == resultState)
            {
                //判定結果のエフェクトを出す
                m_ResultTextDictionary[hitResult].SetActive(false);
                m_ResultTextDictionary[hitResult].SetActive(true);

                Observable.Timer(TimeSpan.FromMilliseconds(200))
                    .Subscribe(_ => m_ResultTextDictionary[hitResult].SetActive(false));

                m_ScoreController.CalculateScore(hitResult);
            }
        }

    }
}