using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicMasterdata", menuName = "ScriptableObjects/CreateMusicMasterData")]
public class MusicMasterData : ScriptableObject
{
    [SerializeField] private MusicData[] m_MusicDataList;

    //staticにするとメモリ確保することになっちゃうな？いちいちロードした方が健全か？
    private static MusicMasterData m_Instance;

    public static MusicMasterData Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load<MusicMasterData>("MusicMasterData");
            }
            return m_Instance;
        }
    }

    public MusicData[] MusicDataList
    {
        get
        {
            return m_MusicDataList;
        }
    }
}
