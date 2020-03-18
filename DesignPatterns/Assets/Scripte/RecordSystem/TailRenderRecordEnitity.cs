using System.Collections.Generic;
using UnityEngine;

public class TailRenderRecordEnitity : RecordEnitity
{
    public TrailRenderer trail;
    public bool lastTrailEmitState = false;
    public List<TrailStateInfo> trailStates = new List<TrailStateInfo>();
    public int stateIndex;

    public override void Start()
    {
        base.Start();
        trail = GetComponent<TrailRenderer>();
    }

    public override void PerBeginRecord()
    {
        base.PerBeginRecord();
        if (trail == null)
        {
            trail = GetComponent<TrailRenderer>();
            if (trail == null)
            {
                Debug.LogError("Error trail is null  => " + trail.gameObject.name);
                enabled = false;
            }
        }
        trailStates = new List<TrailStateInfo>();
        lastTrailEmitState = trail.emitting;
        trailStates.Add(new TrailStateInfo(trail, 0));
    }

    public override void Record(float time)
    {
        if (lastTrailEmitState != trail.emitting)
        {
            trailStates.Add(new TrailStateInfo(trail, time));
        }
    }

    public override void PerReplayInit()
    {
        base.PerReplayInit();
        stateIndex = 0;
        trail.emitting = trailStates[stateIndex].activeState;
    }

    public override void RePlay(float time, float timeScale)
    {
        base.RePlay(time, timeScale);

        if (trailStates.Count <= 0)
        {
            return;
        }
        if (stateIndex < trailStates.Count - 1 && time >= trailStates[stateIndex].nextChangeTime)
        {
            stateIndex++;
            trail.emitting = trailStates[stateIndex].activeState;
        }
    }

    public override void ReplayEnd()
    {
        base.ReplayEnd();
        trailStates = new List<TrailStateInfo>();
        stateIndex = 0;
    }
}