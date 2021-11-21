public class DataManager : Singleton<DataManager>
{
    private MusicData m_MusicData;
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
    
}
