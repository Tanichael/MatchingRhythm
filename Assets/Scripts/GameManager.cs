using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string m_FilePath;
    [SerializeField] private string m_ClipPath;
    [SerializeField] private Button m_Play;
    [SerializeField] private Button m_SetChart;

    [SerializeField] GameObject m_Beautiful;
    [SerializeField] GameObject m_Ugly;

    [SerializeField] Transform m_BaseSpawnPoint;
    [SerializeField] Transform m_BaseBeatPoint;
    [SerializeField] AudioSource m_AudioSource;

    private readonly float ms_Range = 1.7f;
    private readonly float ms_MarginTime = 2 * 1000f;

    private List<Note> m_Notes;
    private bool m_IsPlaying;
    private float m_StartTime;
    private float m_Distance;
    private int m_NoteIndex;

    private void OnEnable()
    {
        m_Distance = Math.Abs(m_BaseBeatPoint.position.y - m_BaseSpawnPoint.position.y);
        m_IsPlaying = false;
        m_NoteIndex = 0;

        m_Play.onClick
            .AsObservable()
            .Subscribe(_ => Play());

        m_SetChart.onClick
            .AsObservable()
            .Subscribe(_ => LoadChart());

        this.UpdateAsObservable()
            .Where(_ => m_IsPlaying)
            .Where(_ => m_Notes.Count > m_NoteIndex)
            .Where(_ => m_Notes[m_NoteIndex].Timing <= ((Time.time * 1000 - m_StartTime) + ms_MarginTime))
            .Subscribe(_ =>
            {
                m_Notes[m_NoteIndex].NoteController.Fire(m_Distance, ms_MarginTime);
                m_NoteIndex++;
            });
        
            
    }

    private void LoadChart()
    {
        m_Notes = new List<Note>();

        string jsonText = Resources.Load<TextAsset>(m_FilePath).ToString();
        m_AudioSource.clip = (AudioClip)Resources.Load(m_ClipPath);

        JsonNode json = JsonNode.Parse(jsonText);

        foreach(var noteData in json["notes"])
        {
            float timing = float.Parse(noteData["timing"].Get<string>());
            int place = int.Parse(noteData["place"].Get<string>());
            string type = noteData["type"].Get<string>();

            Note note = new Note(timing, place, type);

            Vector3 spawnPoint = new Vector3();
            spawnPoint = m_BaseSpawnPoint.position + new Vector3(ms_Range * (place-1f), 0f, 0f);

            if (type == "beautiful")
            {
                note.NoteController.SetUp(m_Beautiful, spawnPoint);
            }
            else if (type == "ugly")
            {
                note.NoteController.SetUp(m_Ugly, spawnPoint);
            }
            else
            {
                note.NoteController.SetUp(m_Beautiful, spawnPoint);
            }

            m_Notes.Add(note);

        }

    }

    private void Play()
    {
        m_AudioSource.Stop();
        m_AudioSource.Play();
        m_StartTime = Time.time * 1000;
        m_IsPlaying = true;
        Debug.Log("Start!!");
    }

}
