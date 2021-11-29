using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboRateMasterData", menuName = "ScriptableObjects/CreateComboRateMasterData")]
public class ComboRateMasterData : ScriptableObject
{
    [SerializeField] private ComboRate[] m_ComboRates;

    //staticにするとメモリ確保することになっちゃうな？いちいちロードした方が健全か？
    private static ComboRateMasterData m_Instance;

    public static ComboRateMasterData Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load<ComboRateMasterData>("ComboRateMasterData");
            }
            return m_Instance;
        }
    }

    public float GetComboRate(float ratio)
    {
        float rate = 1f;

        ComboRate[] comboRates = m_ComboRates;

        foreach (var comboRate in comboRates)
        {
            if (ratio > comboRate.Ratio)
            {
                break;
            }
            rate = comboRate.Rate;
        }

        return rate;
    }

    public ComboRate[] ComboRates
    {
        get
        {
            return m_ComboRates;
        }
    }

}
