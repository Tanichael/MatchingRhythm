using System.Collections.Generic;

public class DataManager : Singleton<DataManager>
{
    private MusicData m_MusicData;
    private float m_Score = 0f;
    private int m_Combo = 0;
    private Dictionary<HitResult.ResultState, int> m_CountDictionary;
    private int m_MaxCombo = 0;

    public MusicData MusicData
    {
        get
        {
            if(m_MusicData == null)
            {
                m_MusicData = MusicMasterData.Instance.MusicDataList[0];
            }
            return m_MusicData;
        }
        set
        {
            m_MusicData = value;
        }
    }

    public float Score
    {
        get
        {
            return m_Score;
        }
        set
        {
            m_Score = value;
        }
    }

    public int Combo
    {
        get
        {
            return m_Combo;
        }
        set
        {
            m_Combo = value;
        }
    }

    public Dictionary<HitResult.ResultState, int> CountDictionary
    {
        get
        {
            if(m_CountDictionary == null)
            {
                m_CountDictionary = new Dictionary<HitResult.ResultState, int>();
            }
            return m_CountDictionary;
        }
        set
        {
            m_CountDictionary = value;

        }
    }

    public int MaxCombo
    {
        get
        {
            return m_MaxCombo;
        }
        set
        {
            m_MaxCombo = value;
        }
    }

}
