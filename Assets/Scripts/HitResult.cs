using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルトに対応するものをまとめておいたクラス マスターデータとしてリストで保管する
/// エフェクトなどもここに入れちゃっていいかも
/// </summary>

[Serializable]
public class HitResult
{
    [SerializeField] private int m_Id;
    [SerializeField] private string m_Result;
    [SerializeField] private float m_BaseScore;
    [SerializeField] private GameObject m_ResultObject;

    public int Id
    {
        get
        {
            return m_Id;
        }
    }

    public string Result
    {
        get
        {
            return m_Result;
        }
    }

    public float BaseScore
    {
        get
        {
            return m_BaseScore;
        }
    }

    public GameObject ResultObject
    {
        get
        {
            return m_ResultObject;
        }
    }

}
