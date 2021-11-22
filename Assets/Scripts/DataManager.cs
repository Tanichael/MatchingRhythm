using System.Collections.Generic;

public class DataManager : Singleton<DataManager>
{
    private MusicData m_MusicData;
    private float m_Score;
    private int m_Combo;
    private Dictionary<string, int> m_CountDictionary;

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

    public Dictionary<string, int> CountDictionary
    {
        get
        {
            if(m_CountDictionary == null)
            {
                m_CountDictionary = new Dictionary<string, int>();
            }
            return m_CountDictionary;
        }
        set
        {
            m_CountDictionary = value;

        }
    }

}
