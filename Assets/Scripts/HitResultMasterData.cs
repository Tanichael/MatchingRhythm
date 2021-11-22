using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitResultMasterData", menuName = "ScriptableObjects/CreateHitResultMasterData")]
public class HitResultMasterData : ScriptableObject
{
    [SerializeField] private HitResult[] m_HitResults;

    //staticにするとメモリ確保することになっちゃうな？いちいちロードした方が健全か？
    private static HitResultMasterData m_Instance;

    public static HitResultMasterData Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load<HitResultMasterData>("HitResultMasterData");
            }
            return m_Instance;
        }
    }

    public HitResult[] HitResults
    {
        get
        {
            return m_HitResults;
        }
    }
}
