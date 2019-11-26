using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRecordEnitity : RecordEnitity
{
    public ParticleSystem particle;
    public AnimationCurve TimeCurve;
    public List<emissionInfo> emissionState = new List<emissionInfo>();

    [System.Serializable]
    public class emissionInfo
    {
        public bool enable;
        public bool emissionEnable;
        public float changeTime;

        public emissionInfo(ParticleSystem target, float change)
        {
            enable = target.gameObject.activeInHierarchy;
            emissionEnable = target.emission.enabled;
            changeTime = change;
        }
    }

    public bool RecordColor = false;
    public bool RecordEmissColor = false;
    public string EmissColorName = "_EmissionColor";
    public List<particleInfo> particleState = new List<particleInfo>();
    public Dictionary<Color, int> ColorDic = new Dictionary<Color, int>();
    public List<particleInfo> particleEmissState = new List<particleInfo>();
    public Dictionary<Color, int> ColorEmissDic = new Dictionary<Color, int>();
    private ParticleSystemRenderer psr;

    [System.Serializable]
    public class particleInfo
    {
        public int colorIndex;
        public float nextChangeTime;

        public particleInfo(int index, float changeTime)
        {
            colorIndex = index;
            nextChangeTime = changeTime;
        }
    }

    public float startRecordTime;
    public float endRecordTime;
    public bool lastEmissionState;
    public Color lastColorState;
    public Color lastColorEmissState;
    public int replayIndex;
    public int replayColorIndex;
    public int replayColorEmissIndex;

    public override void Start()
    {
        particle = GetComponent<ParticleSystem>();
        base.Start();
    }

    public override void PerBeginRecord()
    {
        base.PerBeginRecord();
        emissionState = new List<emissionInfo>();
        particleState = new List<particleInfo>();
        particleEmissState = new List<particleInfo>();
        TimeCurve = new AnimationCurve();
        emissionState.Add(new emissionInfo(particle, 0));
        if (RecordColor)
        {
            ColorDic = new Dictionary<Color, int>();
            lastColorState = particle.startColor;
            ColorDic.Add(lastColorState, ColorDic.Count);
        }
        if (RecordEmissColor)
        {
            ColorEmissDic = new Dictionary<Color, int>();
            psr = particle.GetComponent<ParticleSystemRenderer>();
            if (psr == null)
            {
                RecordEmissColor = false;
            }
            else
            {
                lastColorEmissState = psr.material.GetColor(EmissColorName);
                ColorEmissDic.Add(lastColorEmissState, ColorEmissDic.Count);
            }
        }
        TimeCurve.AddKey(0, particle.time);
    }

    public override void Record(float time)
    {
        Data.Add(transform, time);
        emissionState.Add(new emissionInfo(particle, time));
        if (RecordColor)
        {
            Color tmp = particle.startColor;
            if (ColorDic.ContainsKey(tmp) == false)
            {
                ColorDic.Add(tmp, ColorDic.Count);
            }
            particleState.Add(new particleInfo(ColorDic[tmp], time));
        }
        if (RecordEmissColor)
        {
            Color tmpColor = psr.material.GetColor(EmissColorName);
            if (ColorEmissDic.ContainsKey(tmpColor) == false)
            {
                ColorEmissDic.Add(tmpColor, ColorEmissDic.Count);
            }
            particleEmissState.Add(new particleInfo(ColorEmissDic[tmpColor], time));
        }

        TimeCurve.AddKey(time, particle.time);
    }

    public override void EndRecord(float time)
    {
        endRecordTime = time;
        startRecordTime = RecordManager.Instance.startTime;
        TimeCurve.AddKey(RecordManager.Instance.GetCurrentTime(), particle.time);
    }

    public override void PerReplayInit()
    {
        base.PerReplayInit();
        replayIndex = 0;
        replayColorIndex = 0;
        replayColorEmissIndex = 0;
        lastEmissionState = particle.emission.enabled;
        particle.Pause();
        if (emissionState.Count > 0)
        {
            particle.gameObject.SetActive(emissionState[replayIndex].enable);
            var tmp = particle.emission;
            tmp.enabled = emissionState[replayIndex].emissionEnable;
        }
        if (RecordColor)
        {
            ChangeColor();
        }
        if (RecordEmissColor)
        {
            ChangeColorEmiss();
        }

        particle.Simulate(TimeCurve.Evaluate(startRecordTime), true, false);
        particle.Play(true);
    }

    public override void RePlay(float time)
    {
        Data.Set(time, this.transform);
        if (emissionState.Count > 0 && replayIndex < emissionState.Count - 1 && time >= emissionState[replayIndex].changeTime)
        {
            replayIndex++;
            particle.gameObject.SetActive(emissionState[replayIndex].enable);
            var tmp = particle.emission;
            tmp.enabled = emissionState[replayIndex].emissionEnable;
        }

        if (RecordColor)
        {
            if (particleState == null || particleState.Count == 0)
            {
                return;
            }
            doCheckColorEvent(
                particleState.Count,
                ref replayColorIndex,
                time,
                particleState[replayColorIndex].nextChangeTime,
                ChangeColor
            );
        }
        if (RecordEmissColor)
        {
            if (particleEmissState == null || particleEmissState.Count == 0)
            {
                return;
            }

            if (replayColorEmissIndex < particleEmissState.Count)
            {
                doCheckColorEvent(
                particleEmissState.Count,
                ref replayColorEmissIndex,
                time,
                particleEmissState[replayColorEmissIndex].nextChangeTime,
                ChangeColorEmiss
            );
            }
        }
    }

    private void doCheckColorEvent(int listLength, ref int index, float time, float nextTime, Action call)
    {
        if (listLength > 0 && index < listLength - 1 && time >= nextTime)
        {
            index++;
            call();
        }
    }

    private void ChangeColor()
    {
        if (particleState.Count < 1)
        {
            return;
        }
        var tmp = particle.main;
        float colorId = particleState[replayColorIndex].colorIndex;
        foreach (Color color in ColorDic.Keys)
        {
            if (ColorDic[color] == colorId)
            {
                tmp.startColor = color;
                break;
            }
        }
    }

    private void ChangeColorEmiss()
    {
        if (particleEmissState.Count < 1)
        {
            return;
        }
        float colorId = particleEmissState[replayColorEmissIndex].colorIndex;
        foreach (Color color in ColorEmissDic.Keys)
        {
            if (ColorEmissDic[color] == colorId)
            {
                psr.material.SetColor(EmissColorName, color);
                break;
            }
        }
    }

    public override void ReplayEnd()
    {
        base.ReplayEnd();
        var tmp = particle.emission;
        tmp.enabled = lastEmissionState;
        if (RecordColor)
        {
            var tmpMain = particle.main;
            tmpMain.startColor = lastColorState;
        }
        if (RecordEmissColor)
        {
            psr.material.SetColor(EmissColorName, lastColorEmissState);
        }
        replayColorIndex = 0;
        replayColorEmissIndex = 0;
    }
}