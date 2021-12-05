using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullComboEffect : MonoBehaviour
{
    [SerializeField] private float m_Speed = 1.0f;
    [SerializeField] private Text m_Text;
    [SerializeField] private AudioSource m_FullComboAudio;

    private readonly float ms_WaitTime = 0.8f;

    public AudioSource FullComboAudio
    {
        get
        {
            return m_FullComboAudio;
        }
    }

    private float m_Time = 0f;

    private void Update()
    {
        RectTransform rectTransform = this.gameObject.transform as RectTransform;
        if(rectTransform != null)
        {
            rectTransform.localScale = GetScale();
        }
        m_Text.color = GetAlpha(m_Text.color);
        if(GetScale().x > 10f)
        {
            Destroy(this.gameObject);
        }
    }

    private Vector3 GetScale()
    {
        Vector3 scale = new Vector3(1f, 1f, 0);
        m_Time += Time.deltaTime * m_Speed;
        if (m_Time < ms_WaitTime) return scale;
        scale = new Vector3(1f + (m_Time - ms_WaitTime) * (m_Time - ms_WaitTime), 1f + (m_Time - ms_WaitTime) * (m_Time - ms_WaitTime), 1f);

        return scale;
    }

    private Color GetAlpha(Color color)
    {
        m_Time += Time.deltaTime * m_Speed;
        if (m_Time < ms_WaitTime) return color;
        color.a = 1.0f - (m_Time - ms_WaitTime) * (m_Time - ms_WaitTime) / 10f;

        return color;
    }
}
