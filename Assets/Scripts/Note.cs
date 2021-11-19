using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    public enum State
    {
        On,
        Off
    }

    private State m_NoteState;

    private float m_Timing; //タイミング
    private int m_Place; //位置 -1:左, 0:中央, 1:右
    private string m_Type; //ノーツのタイプ beautiful, ugly
    private NoteController m_NoteController;
    private GameObject m_NoteObject;


    public Note(float timing, int place, string type)
    {
        m_NoteState = State.Off;
        m_Timing = timing;
        m_Place = place;
        m_Type = type;
        m_NoteController = new NoteController(this);
    }

    public float Timing
    {
        get
        {
            return m_Timing;
        }
    }

    public int Place
    {
        get
        {
            return m_Place;
        }
    }

    public string Type
    {
        get
        {
            return m_Type;
        }
    }

    public State NoteState
    {
        get
        {
            return m_NoteState;
        }
        set
        {
            m_NoteState = value;
        }
    }

    public NoteController NoteController
    {
        get
        {
            return m_NoteController;
        }
    }

    public GameObject NoteObject
    {
        get
        {
            return m_NoteObject;
        }
        set
        {
            m_NoteObject = value;
        }
    }

}
