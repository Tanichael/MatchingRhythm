using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HitNotesManager : MonoBehaviour
{
    [SerializeField] GameManager m_GameManager;

    private HitResult[] m_HitResults;

    private void OnEnable()
    {
        m_HitResults = HitResultMasterData.Instance.HitResults;

        m_GameManager
            .OnHitNotes
            .Subscribe(result => OnHitNotes(result));

    }

    //判定に応じて処理をする関数
    //引数をstringにしているが、判定をEnumにしてそれを引数にした方が硬い動きしそう
    private void OnHitNotes(string result)
    {
        //resultをもとに判定がどれか探す
        foreach(var hitResult in m_HitResults)
        {
            if(hitResult.Result == result)
            {
                GameObject resultObject = Instantiate(hitResult.ResultObject);
                resultObject.SetActive(false);
                resultObject.SetActive(true);

                Observable.Timer(TimeSpan.FromMilliseconds(200))
                    .Subscribe(_ => resultObject.SetActive(false));
            }
        }
    }
}
