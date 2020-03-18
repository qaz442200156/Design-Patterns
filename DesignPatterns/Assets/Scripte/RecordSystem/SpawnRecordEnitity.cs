using System.Collections.Generic;
using UnityEngine;

public class SpawnRecordEnitity : RecordEnitity
{
    public bool uesNormalSpawner = false;
    public List<SpwanStateInfo> spawnList = new List<SpwanStateInfo>();
    public bool useEffectSpawner = false;
    public List<EffectSpawnerStateInfo> effectSpawnList = new List<EffectSpawnerStateInfo>();

    public override void Start()
    {
        base.Start();
    }

    public override void PerBeginRecord()
    {
        base.PerBeginRecord();
    }

    public void AddToRecord(GameObject target, float time, float destoryTime)
    {
        if (uesNormalSpawner == false || RecordManager.Instance.isReplaying)
        {
            return;
        }
        if (RecordManager.Instance.isRecording)
        {
            spawnList.Add(new SpwanStateInfo(target, time, destoryTime));
        }
    }

    public void AddToRecord(string target, float time, Vector3 spawnPos, GameObject parent, bool useLocal)
    {
        if (useEffectSpawner == false || RecordManager.Instance.isReplaying)
        {
            return;
        }
        if (RecordManager.Instance.isRecording)
        {
            effectSpawnList.Add(new EffectSpawnerStateInfo(target, time, spawnPos, parent, useLocal));
        }
    }

    public override void PerReplayInit()
    {
        base.PerReplayInit();
    }

    public override void RePlay(float time, float timeScale)
    {
        if (uesNormalSpawner && spawnList.Count > 0)
        {
            foreach (SpwanStateInfo info in spawnList)
            {
                if (info.isSpawned)
                {
                    continue;
                }
                if (time >= info.spawnTime)
                {
                    info.Spawn();
                }
            }
        }
        if (useEffectSpawner && effectSpawnList.Count > 0)
        {
            foreach (EffectSpawnerStateInfo info in effectSpawnList)
            {
                if (info.isSpawned)
                {
                    continue;
                }
                if (time >= info.spawnTime)
                {
                    info.Spawn();
                }
            }
        }
    }

    public override void ReplayEnd()
    {
        base.ReplayEnd();
        foreach (SpwanStateInfo info in spawnList)
        {
            info.CheckAndClear();
        }
        foreach (EffectSpawnerStateInfo info in effectSpawnList)
        {
            info.CheckAndClear();
        }
    }
}