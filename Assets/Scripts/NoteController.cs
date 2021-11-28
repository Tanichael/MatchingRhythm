using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ノーツの動きについてまとめたクラス
/// </summary>
public class NoteController
{
    private readonly float ms_NoteStartScale = 0.2f;

    private Note m_Note;
    private Vector3 m_FirstPos;

    private float m_Distance;
    private float m_Range;
    private float m_MarginTime;

    private float m_GoTime;

    public NoteController(Note note)
    {
        m_Note = note;
    }


    public Note Note
    {
        get
        {
            return m_Note;
        }
    }

    //Instantiateを担当するクラス
    public void SetUp(GameObject notePrefab, Vector3 position)
    {
        GameObject noteObject = GameObject.Instantiate(notePrefab, position, Quaternion.identity);
        m_Note.NoteObject = noteObject;
        m_FirstPos = m_Note.NoteObject.transform.position;

        m_Note.NoteObject.UpdateAsObservable()
            .Where(_ => m_Note.IsRunning == true)
            .Subscribe(_ =>
            {
                //m_Note.NoteObject.transform.position = new Vector3(m_FirstPos.x, m_FirstPos.y - m_Distance * (Time.time * 1000 - m_GoTime) / m_MarginTime, m_FirstPos.z);
                m_Note.NoteObject.transform.position = new Vector3(m_FirstPos.x + (m_Note.Place - 1) * 1.5f * (Time.time * 1000 - m_GoTime) / m_MarginTime, m_FirstPos.y - m_Distance * (Time.time * 1000 - m_GoTime) / m_MarginTime, m_FirstPos.z);

                //m_Note.NoteObject.transform.localScale =
                //    new Vector3(
                //        ms_NoteStartScale + (1f - ms_NoteStartScale) * Mathf.Sin(Mathf.PI / 2 * (Time.time * 1000 - m_GoTime) / m_MarginTime),
                //        ms_NoteStartScale + (1f - ms_NoteStartScale) * Mathf.Sin(Mathf.PI / 2 * (Time.time * 1000 - m_GoTime) / m_MarginTime),
                //        1);

                m_Note.NoteObject.transform.localScale =
                    new Vector3(
                        ms_NoteStartScale + (1f - ms_NoteStartScale) * (Time.time * 1000 - m_GoTime) / m_MarginTime,
                        ms_NoteStartScale + (1f - ms_NoteStartScale) * (Time.time * 1000 - m_GoTime) / m_MarginTime,
                        1);
            });
    }

    public void Fire(float distance, float range, float marginTime)
    {
        m_Distance = distance;
        m_Range = range;
        m_MarginTime = marginTime;
        m_GoTime = Time.time * 1000;

        m_Note.NoteState = Note.State.On;
        m_Note.IsRunning = true;
    }


}
