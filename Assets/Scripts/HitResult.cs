﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ノーツヒット時のリザルトに対応するものをまとめておいたクラス マスターデータとしてリストで保管する
/// 判定ごとに表示するオブジェクトやスコア倍率などが記録されている
/// エフェクトなどもここに入れちゃっていいかも
/// </summary>

[Serializable]
public class HitResult
{
    public enum ResultState
    {
        Perfect = 0, //リザルト画面のカウントテキストの扱いの時に配列で管理できるようindexをつけておく
        Good = 1,
        Nice = 2,
        Bad = 3
    }

    [SerializeField] private int m_Id;
    [SerializeField] private ResultState m_State;
    [SerializeField] private float m_ScoreRate;
    [SerializeField] private GameObject m_ResultObject;
    [SerializeField] private float m_BeatRange;

    public int Id
    {
        get
        {
            return m_Id;
        }
    }

    public ResultState State
    {
        get
        {
            return m_State;
        }
    }

    public float ScoreRate
    {
        get
        {
            return m_ScoreRate;
        }
    }

    public GameObject ResultObject
    {
        get
        {
            return m_ResultObject;
        }
    }

    public float BeatRange
    {
        get
        {
            return m_BeatRange;
        }
    }

}
