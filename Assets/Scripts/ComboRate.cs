using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComboRate
{
    [SerializeField] private float m_Ratio; //0 ~ 1で全ノーツ数に対する比率を定義。その比率までは m_Rate がスコアにかかる
    [SerializeField] private float m_Rate; //コンボ倍率

    public float Ratio
    {
        get
        {
            return m_Ratio;
        }
    }

    public float Rate
    {
        get
        {
            return m_Rate;
        }
    }
}
