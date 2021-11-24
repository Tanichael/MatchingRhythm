using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

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

    private readonly float ms_Range = 1.4f;
    private readonly float ms_JudgeRange = 0.8f;
    private readonly float ms_MarginTime = 1200f;
    private readonly float ms_CheckRange = 120f;
    private readonly float ms_BeatRange = 80f;
    private readonly float ms_FlickStartRange = 0.1f;
    private readonly float ms_FlickRange = 0.2f;

    private List<Note> m_Notes;
    private bool m_IsPlaying;
    private float m_GameStartTime;
    private float m_Distance;
    private int m_NoteIndex;

    //フリック関連メンバー変数
    private Vector3 m_StartMousePos;
    private Vector3 m_EndPos;
    private float m_FlickStartTiming;
    private float m_FlickEndTiming;
    private bool m_DoStartFlick;

    //音楽関連メンバー変数
    private bool m_IsPause = false; //ポーズされてるか

    private Subject<string> m_LoadMusicDataSubject = new Subject<string>(); //subject

    public IObservable<string> OnLoadMusicData //observer
    {
        get
        {
            return m_LoadMusicDataSubject;
        }
    }

    //SoundEffectを管理するsubject
    private Subject<string> SoundEffectSubject = new Subject<string>();

    public IObservable<string> OnSoundEffect
    {
        get
        {
            return SoundEffectSubject;
        }
    }

    //ノーツのヒットを管理するサブジェクト
    private Subject<HitResult.ResultState> m_HitNotesSubject = new Subject<HitResult.ResultState>();

    public IObservable<HitResult.ResultState> OnHitNotes
    {
        get
        {
            return m_HitNotesSubject;
        }
    }

    private void OnEnable()
    {
        m_Distance = Math.Abs(m_BaseBeatPoint.position.y - m_BaseSpawnPoint.position.y);
        m_IsPlaying = false;
        m_NoteIndex = 0;
        m_DoStartFlick = false;

        //int place = -1;
        //int nearNoteIndex = -1;

        //ノーツ読み込み後の処理 loaderクラスを用意してもいい？
        this
            .OnLoadMusicData
            .Where(load => load == "load")
            .Subscribe(load => Play());

        //m_Play.onClick
        //    .AsObservable()
        //    .Subscribe(_ => Play());

        //m_SetChart.onClick
        //    .AsObservable()
        //    .Subscribe(_ => LoadChart());

        this.UpdateAsObservable()
            .Where(_ => m_IsPlaying)
            .Where(_ => m_Notes.Count > m_NoteIndex)
            .Where(_ => m_Notes[m_NoteIndex].Timing <= ((Time.time * 1000 - m_GameStartTime) + ms_MarginTime))
            .Subscribe(_ =>
            {
                m_Notes[m_NoteIndex].NoteController.Fire(m_Distance, ms_MarginTime);
                m_NoteIndex++;
            });

        //TODO: ここをGetMouseButtonにして、ポジションがノーツヒット位置に近いかどうかで判定する
        //ここでもノーツ検索処理入れて近くにノーツがあるかどうかを確認した方が良さそう
        //煩雑になってきたからJudgeControllerクラスとか作ってまとめた方がいいかも?
        this.UpdateAsObservable()
            .Where(_ => m_IsPlaying)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                m_StartMousePos = Input.mousePosition;
                m_FlickStartTiming = Time.time * 1000;
                m_DoStartFlick = true; //フリックスタートフラグを立てる
            });

        //this.UpdateAsObservable()
        //    .Where(_ => m_IsPlaying)
        //    .Where(_ => m_DoStartFlick == false)
        //    .Where(_ => Input.GetMouseButton(0))
        //    .Where(_ =>
        //    {
        //        place = NearHitPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
        //        nearNoteIndex = NearNoteExist(Time.time * 1000 - m_GameStartTime);
        //        return place != -1 && nearNoteIndex != -1;
        //    })
        //    .Where(_ =>
        //    {
        //        return m_Notes[nearNoteIndex].NoteState == Note.State.On; //判定されてないものだけ
        //    })
        //    .Subscribe(_ =>
        //    {
        //        m_StartMousePos = Input.mousePosition;
        //        m_FlickStartTiming = Time.time * 1000;
        //        m_DoStartFlick = true; //フリックスタートフラグを立てる
        //        Debug.Log("FlickStart");
        //    });

        this.UpdateAsObservable()
            .Where(_ => m_IsPlaying)
            .Where(_ => m_DoStartFlick == true) //フリックスタートしてるか
            .Where(_ => Input.GetMouseButton(0))
            .Where(_ => Mathf.Abs(Input.mousePosition.x - m_StartMousePos.x) >= ms_FlickRange) //フリックしたか否か
            .Subscribe(_ =>
            {
                Debug.Log("FlickEnd");
                m_EndPos = Input.mousePosition;
                m_FlickEndTiming = Time.time * 1000;

                float timing;
                string type;
                int place;

                //placeの判定処理
                place = CalcPlace(m_StartMousePos);

                //タイミングの判定処理
                timing = (m_FlickEndTiming + m_FlickStartTiming) / 2 - m_GameStartTime;

                if (m_EndPos.x - m_StartMousePos.x > 0) //右フリックの時
                {
                    type = "beautiful";
                    beat(timing, place, type);
                    SoundEffectSubject.OnNext(type);
                }
                else if(m_EndPos.x - m_StartMousePos.x < 0) //左フリックの時
                {
                    type = "ugly";
                    beat(timing, place, type);
                    SoundEffectSubject.OnNext(type);
                }
                
                m_DoStartFlick = false; //フリック完了をフラグで管理
            });

        //再生が終わった時の処理
        this.UpdateAsObservable()
            .Where(_ => m_IsPlaying == true) //ゲームがスタートした後かどうか
            .Where(_ => m_IsPause == false)
            .Where(_ => m_AudioSource.isPlaying == false)
            .Subscribe(_ =>
            {
                //とりあえず遅らせてからシーン遷移
                Observable.Timer(TimeSpan.FromMilliseconds(2000))
                    .Subscribe(__ => SceneLoader.Instance.GoSceneAsync("ResultScene").Forget());
            });

        LoadChart(); //ノーツ読み込み

    }

    private void LoadChart()
    {
        m_Notes = new List<Note>();

        //音楽データをセット
        //ここの処理は選択画面の方に移すことになりそう
        string jsonText = DataManager.Instance.MusicData.NotesData.ToString();
        m_AudioSource.clip = DataManager.Instance.MusicData.AudioClip;

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
            else if (type == "none")
            {
                continue;
            }
            else
            {
                note.NoteController.SetUp(m_Beautiful, spawnPoint);
            }

            m_Notes.Add(note);
        }

        m_LoadMusicDataSubject.OnNext("load"); //ロード完了を伝える
        Debug.Log("load");

    }

    private void Play()
    {
        m_AudioSource.Stop();
        m_AudioSource.Play();
        m_GameStartTime = Time.time * 1000;
        m_IsPlaying = true;
        Debug.Log("Start!!");
    }

    //本当はこの中の詳細な処理は別のところに委譲したい
    private void beat(float timing, int place, string type)
    {
        float minDiff = -1f;
        int minDiffIndex = -1;

        //該当するノーツを探す処理
        for(int i = 0; i < m_Notes.Count; i++)
        {
            if (m_Notes[i].Timing > 0)
            {
                float diff = Math.Abs(m_Notes[i].Timing - timing);
                if (minDiff == -1 || minDiff > diff)
                {
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }

        if(minDiff != -1 & minDiff < ms_CheckRange) //スルーしてない時
        {
            if(minDiff < ms_BeatRange & m_Notes[minDiffIndex].Type == type & m_Notes[minDiffIndex].Place == place)
            {
                m_Notes[minDiffIndex].Timing = -1;
                m_Notes[minDiffIndex].NoteObject.SetActive(false); //Noteクラスのstateを変えるとかでは？
                m_Notes[minDiffIndex].NoteState = Note.State.Off;

                m_HitNotesSubject.OnNext(HitResult.ResultState.Good); //ここのメッセージとマスターデータのResultを揃えなきゃいけない
            }
            else
            {
                m_Notes[minDiffIndex].Timing = -1;
                m_Notes[minDiffIndex].NoteObject.SetActive(false);
                m_Notes[minDiffIndex].NoteState = Note.State.Off;

                m_HitNotesSubject.OnNext(HitResult.ResultState.Failure);
            }
        }
        else //スルーしてる時
        {
            m_Notes[minDiffIndex].NoteState = Note.State.Off;
            Debug.Log("through");
        }
    }

    //ピクセルの座標を与えられたときにplaceに変換して返す関数
    private int CalcPlace(Vector3 startMousePos)
    {
        int place = -1;

        Vector3 startPos = Camera.main.ScreenToWorldPoint(startMousePos);

        //placeの判定処理
        if (startPos.x < -ms_JudgeRange)
        {
            place = 0;
        }
        else if (startPos.x > ms_JudgeRange)
        {
            place = 2;
        }
        else
        {
            place = 1; //とりあえず他の時は1ってことにしとく
        }

        return place;
    }

    private int NearHitPosition(float x)
    {
        int place = -1;

        //xをもとにplaceに近いかどうか判定
        //ここは結構シビアに取った方がいい

        for(int i = 0; i < 3; i++)
        {
            float hitPosition = ms_Range * (i - 1);
            if(Mathf.Abs(x - hitPosition) < ms_FlickStartRange)
            {
                place = i;
            }
        }

        return place;
    }

    private int NearNoteExist(float timing)
    {
        int index = -1;

        float minDiff = -1f;
        int minDiffIndex = -1;

        //該当するノーツを探す処理
        for (int i = 0; i < m_Notes.Count; i++)
        {
            if (m_Notes[i].Timing > 0)
            {
                float diff = Math.Abs(m_Notes[i].Timing - timing);
                if (minDiff == -1 || minDiff > diff)
                {
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }

        if(minDiff != -1 && minDiff < ms_BeatRange)
        {
            index = minDiffIndex;
        }
        return index;
    }

}
