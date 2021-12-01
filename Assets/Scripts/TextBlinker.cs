using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlinker : MonoBehaviour
{
    [SerializeField] private float m_Speed = 1.0f;
    [SerializeField] private Text[] m_Texts;

    private float m_Time;

    // Update is called once per frame
    private void Update()
    {
        foreach(var text in m_Texts)
        {
            text.color = GetAlpha(text.color);
        }
    }

    private Color GetAlpha(Color color)
    {
        m_Time += Time.deltaTime * m_Speed;
        color.a = Mathf.Sin(m_Time) + 1.0f;

        return color;
    }
}
