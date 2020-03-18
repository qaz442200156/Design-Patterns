using System.Collections.Generic;
using UnityEngine;

public class ActiveStateRecordEnitity : RecordEnitity
{
    public List<GameObject> recordObjects = new List<GameObject>();

    [System.Serializable]
    public class ActiveStatePack
    {
        public bool isEnd = false;
        public int index;
        public bool lastStateWhenReplayStart;
        private bool lastRecordState;
        public GameObject host;
        public List<ActiveStateInfo> recordStates = new List<ActiveStateInfo>();

        public ActiveStatePack(GameObject target)
        {
            host = target;
            recordStates = new List<ActiveStateInfo>();
            lastStateWhenReplayStart = host.activeInHierarchy;
            AddState(host, 0);
            index = 0;
            isEnd = false;
        }

        public void CheckState(float time)
        {
            if (lastRecordState != host.activeInHierarchy)
            {
                AddState(host, time);
            }
        }

        public void AddState(GameObject target, float changeTime)
        {
            lastRecordState = target.activeInHierarchy;
            recordStates.Add(new ActiveStateInfo(target, changeTime));
        }

        public void NextState()
        {
            if (index < recordStates.Count - 1)
            {
                index++;
            }
            if (index == recordStates.Count - 1)
            {
                isEnd = true;
            }
        }

        public bool GetState(float time)
        {
            float nextTime_ = nextTime;
            if (nextTime_ >= 0 && time >= nextTime_)
            {
                NextState();
            }
            return recordStates[index].activeState;
        }

        public float nextTime
        {
            get
            {
                if (isEnd)
                {
                    return -1;
                }
                else
                {
                    return recordStates[index].nextChangeTime;
                }
            }
        }
    }

    public List<ActiveStatePack> stateList = new List<ActiveStatePack>();

    public void AddRecordObject(GameObject target)
    {
        recordObjects.Add(target);
    }

    public override void Start()
    {
        base.Start();
        if (recordObjects.Count <= 0)
        {
            enabled = false;
        }
    }

    public override void PerBeginRecord()
    {
        base.PerBeginRecord();
        stateList = new List<ActiveStatePack>();
        recordObjects.RemoveAll(item => item == null);
        foreach (GameObject obj in recordObjects)
        {
            if (obj != null)
            {
                stateList.Add(new ActiveStatePack(obj));
            }
        }
    }

    public override void Record(float time)
    {
        foreach (ActiveStatePack recordObj in stateList)
        {
            recordObj.CheckState(time);
        }
    }

    public override void EndRecord(float time)
    {
        base.EndRecord(time);
    }

    public override void PerReplayInit()
    {
        base.PerReplayInit();

        foreach (ActiveStatePack recordObj in stateList)
        {
            recordObj.lastStateWhenReplayStart = recordObj.host.activeInHierarchy;
        }
    }

    public override void RePlay(float time,float timeScale)
    {
        foreach (ActiveStatePack recordObj in stateList)
        {
            recordObj.host.SetActive(recordObj.GetState(time));
        }
    }

    public override void ReplayEnd()
    {
        base.ReplayEnd();
        foreach (ActiveStatePack recordObj in stateList)
        {
            recordObj.host.SetActive(recordObj.lastStateWhenReplayStart);
        }
        stateList = new List<ActiveStatePack>();
    }
}