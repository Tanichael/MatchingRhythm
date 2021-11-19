using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class NoteController
{
    private Note m_Note;
    private Vector3 m_FirstPos;

    private float m_Distance;
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
            .Where(_ => m_Note.NoteState == Note.State.On)
            .Subscribe(_ =>
            {
                m_Note.NoteObject.transform.position = new Vector3(m_FirstPos.x, m_FirstPos.y - m_Distance * (Time.time * 1000 - m_GoTime) / m_MarginTime, m_FirstPos.z);
            });
        Debug.Log("Timing = " + m_Note.Timing);
    }

    public void Fire(float distance, float marginTime)
    {
        m_Distance = distance;
        m_MarginTime = marginTime;
        m_GoTime = Time.time * 1000;

        m_Note.NoteState = Note.State.On;
    }


}
