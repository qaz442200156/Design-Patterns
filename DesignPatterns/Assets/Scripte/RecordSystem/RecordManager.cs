using System;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    // TODO Need Fix the Recording Data. It will become bigger and lag when long recording
    public static RecordManager Instance;
    public int recoredRate = 60;
    public bool recordOnAwake;
    public bool isRecording = false;
    public bool isReplaying = false;
    public bool isReplayPause = false;
    public bool needReStart = false;
    public bool stopTimeScaleAtStopOrReplayEnd = true;
    public int replayIndex = 0;
    public int autoStopRecordInSeconds;

    #region Event

    public Action<float,float> OnReplayTimeChange;
    public Action<float> OnEndRecord;
    public Action OnBeginStart;
    public Action OnReplayStart;
    public Action OnReplayEnd;

    #endregion Event

    #region UI

    public Slider slider;

    #endregion UI

    #region Time

    public float _startTime;
    public float startTime;
    public float endTime;
    public float loopTime = 10;

    #endregion Time

    public void Awake()
    {
        Instance = this;
        replayIndex = 0;
        isRecording = false;
    }

    public void Start()
    {
        if (slider)
        {
            slider.onValueChanged.AddListener(v => SetCursor(v));
        }
    }

    public float GetCurrentTime()
    {
        return Time.unscaledTime - _startTime;
    }

    public void isNeedRecordCheck()
    {
        if (isRecording)
        {
            if (needReStart)
            {
                BegineRecord();
            }
        }
        else
        {
            BegineRecord();
        }
    }

    public void BegineRecord()
    {
        if (allowRecord() == false)
        {
            return;
        }
        if (isRecording)
        {
            return;
        }

        if (stopTimeScaleAtStopOrReplayEnd)
        {
            Time.timeScale = 1;
        }
        OnBeginStart();
        isReplaying = false;
        isRecording = true;
        isReplayPause = false;
        startTime = 0;
        _startTime = Time.unscaledTime;
    }

    public void EndRecord()
    {
        if (allowRecord() == false)
        {
            return;
        }
        isRecording = false;
        endTime = Time.unscaledTime - _startTime;
        startTime = Mathf.Max(endTime - loopTime, 0);
        OnEndRecord(endTime);
        if (stopTimeScaleAtStopOrReplayEnd)
        {
            Time.timeScale = 0;
        }
    }

    public void Replay()
    {
        if (allowRecord() == false)
        {
            return;
        }

        Time.timeScale = 1;
        replayIndex = 0;
        isReplaying = true;
        isReplayPause = false;
        startTime = Mathf.Max(endTime - loopTime, 0);
        if (slider)
        {
            slider.value = startTime;
            slider.minValue = startTime;
            slider.maxValue = endTime;
        }
        OnReplayTimeChange(0,Time.timeScale);
        OnReplayStart();
        Time.timeScale = 1;
    }

    public void EndReplay()
    {
        if (allowRecord() == false)
        {
            return;
        }

        isReplaying = false;
        isReplayPause = false;
        OnReplayEnd();
        if (stopTimeScaleAtStopOrReplayEnd)
        {
            Time.timeScale = 0;
        }
    }

    public void Update()
    {
        if (allowRecord() == false)
        {
            return;
        }

        if (recordOnAwake && isRecording == false)
        {
            recordOnAwake = false;
            BegineRecord();
        }

        if (isRecording)
        {
            if (autoStopRecordInSeconds > 0)
            {
                if (Time.unscaledTime - _startTime > autoStopRecordInSeconds)
                {
                    EndRecord();
                }
            }
        }

        if (isReplaying)
        {
            if (isReplayPause == false)
            {
                slider.value += Time.deltaTime * Time.timeScale;
                OnReplayTimeChange(slider.value, Time.timeScale);
                if (slider.value >= slider.maxValue)
                {
                    isReplaying = false;
                    isReplayPause = true;
                    if (stopTimeScaleAtStopOrReplayEnd) { 
                        EndReplay();// For Test need be delete when real to use 
                    }
                }
            }
            else {
                OnReplayTimeChange(slider.value, 0);
            }
        }
    }

    public void ReplayPause() {
        isReplayPause = !isReplayPause;
    }

    public void SetCursor(Single v)
    {
        if (isReplayPause)
        {
            OnReplayTimeChange(v + startTime, Time.timeScale);
        }
    }

    bool allowRecord()
    {
        // You can add other control in here to limit Record system
        return true;
    }
}