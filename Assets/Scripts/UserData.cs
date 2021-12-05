using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : Singleton<UserData>
{
    /// <summary>
    /// 曲のidを受け取ってハイスコアを返す関数
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public float GetHighMoteIndex(int id)
    {
        float highMoteIndex = PlayerPrefs.GetFloat(GetHighMoteIndexKeyFromId(id));
        return highMoteIndex;
    }

    public void SetHighMoteIndex(int id, float moteIndex)
    {
        PlayerPrefs.SetFloat(GetHighMoteIndexKeyFromId(id), moteIndex);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 曲のidを受け取って最大コンボ数を返す関数
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetHighCombo(int id)
    {
        int highCombo = PlayerPrefs.GetInt(GetHighMoteIndexKeyFromId(id));
        return highCombo;
    }

    public void SetHighCombo(int id, int maxCombo)
    {
        PlayerPrefs.SetInt(GetHighComboKeyFromId(id), maxCombo);
        PlayerPrefs.Save();
    }

    private string GetHighMoteIndexKeyFromId(int id)
    {
        string key;
        key = id.ToString() + "_high_mote_index";
        return key;
    }

    private string GetHighComboKeyFromId(int id)
    {
        string key;
        key = id.ToString() + "_high_combo";
        return key;
    }

}
