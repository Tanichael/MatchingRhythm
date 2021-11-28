using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ノーツの動きについてまとめたクラス
/// </summary>
public class NoteController
{
    private readonly float ms_NoteStartScale = 0.2f;
    private readonly float ms_CheckRange = 120f;
    private readonly float ms_BeatRange = 80f;

    private GameManager m_GameManager;

    private Note m_Note;
    private Vector3 m_FirstPos;

    private float m_Distance;
    private float m_MarginTime;

    private float m_GoTime;

    public NoteController(Note note, GameManager gameManager)
    {
        m_Note = note;
        m_GameManager = gameManager;
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
                m_Note.NoteObject.transform.position = new Vector3(m_FirstPos.x, m_FirstPos.y - m_Distance * (Time.time * 1000 - m_GoTime) / m_MarginTime, m_FirstPos.z);
                m_Note.NoteObject.transform.localScale =
                    new Vector3(
                        ms_NoteStartScale + (1f - ms_NoteStartScale) * Mathf.Sin(Mathf.PI / 2 * (Time.time * 1000 - m_GoTime) / m_MarginTime),
                        ms_NoteStartScale + (1f - ms_NoteStartScale) * Mathf.Sin(Mathf.PI / 2 * (Time.time * 1000 - m_GoTime) / m_MarginTime),
                        1);

                //throughしてた時はここでJudge関数を走らせることにすればいい
                if(Time.time * 1000 - m_GoTime - m_MarginTime > ms_CheckRange && m_Note.NoteState == Note.State.On)
                {
                    Debug.Log("bad判定!");
                    Judge(ms_CheckRange, "beautiful"); //Bad判定にさせる
                }
            });
    }

    public void Fire(float distance, float marginTime)
    {
        m_Distance = distance;
        m_MarginTime = marginTime;
        m_GoTime = Time.time * 1000;

        m_Note.NoteState = Note.State.On;
        m_Note.IsRunning = true;
    }

    /// <summary>
    /// resultStateをGameManagerに返すクラス
    /// </summary>
    /// <param name="minDiff">checkRangeを下回ったノーツのタイミングとの差分</param>
    /// <param name="place"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public HitResult.ResultState Judge(float minDiff, string type)
    {
        m_Note.Timing = -1;
        m_Note.NoteObject.SetActive(false);
        m_Note.NoteState = Note.State.Off;

        HitResult.ResultState resultState = HitResult.ResultState.Bad;

        if (type != m_Note.Type)
        {
            m_GameManager.HitNotesSubject.OnNext(resultState);
            return resultState;
        }

        HitResult[] hitResults = HitResultMasterData.Instance.HitResults;

        foreach(var hitResult in hitResults)
        {
            if(minDiff <= hitResult.BeatRange)
            {
                resultState = hitResult.State;
                break;
            }
        }

        m_GameManager.HitNotesSubject.OnNext(resultState);

        return resultState;
    }

}
