using System;
using UnityEngine;

[Serializable]
public class MusicData
{
    [SerializeField] private int m_Id;
    [SerializeField] private string m_Title;
    [SerializeField] private AudioClip m_AudioClip;
    [SerializeField] private TextAsset m_NotesData;
}
