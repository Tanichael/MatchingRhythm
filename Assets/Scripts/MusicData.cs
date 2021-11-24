using System;
using UnityEngine;

[Serializable]
public class MusicData
{
    [SerializeField] private int m_Id;
    [SerializeField] private string m_Title;
    [SerializeField] private AudioClip m_AudioClip;
    [SerializeField] private TextAsset m_NotesData;

    public int Id
    {
        get
        { 
            return m_Id;
        }
    }

    public string Title
    {
        get
        {
            return m_Title;
        }
    }

    public AudioClip AudioClip
    {
        get
        {
            return m_AudioClip;
        }
           
    }

    public TextAsset NotesData
    {
        get
        {
            return m_NotesData;
        }
    }
}
