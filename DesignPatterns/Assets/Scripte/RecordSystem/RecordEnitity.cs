using System.Collections;
using UnityEngine;

///Position Record data
[System.Serializable]
public class TimelineVector
{
    public AnimationCurve x = new AnimationCurve();
    public AnimationCurve y = new AnimationCurve();
    public AnimationCurve z = new AnimationCurve();

    public virtual void Add(Vector3 v, float currentTime)
    {
        float time = currentTime;
        x.AddKey(time, v.x);
        y.AddKey(time, v.y);
        z.AddKey(time, v.z);
    }

    public virtual Vector3 Get(float time)
    {
        return new Vector3(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time));
    }

    public virtual void Clear()
    {
        x = new AnimationCurve();
        y = new AnimationCurve();
        z = new AnimationCurve();
    }
}

///Rotation Record data
[System.Serializable]
public class TimeLineQuaternion
{
    public AnimationCurve x = new AnimationCurve();
    public AnimationCurve y = new AnimationCurve();
    public AnimationCurve z = new AnimationCurve();
    public AnimationCurve w = new AnimationCurve();

    public virtual void Add(Quaternion v, float currentTime)
    {
        float time = currentTime;
        x.AddKey(time, v.x);
        y.AddKey(time, v.y);
        z.AddKey(time, v.z);
        w.AddKey(time, v.w);
    }

    public virtual Quaternion Get(float time)
    {
        return new Quaternion(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time), w.Evaluate(time));
    }

    public virtual void Clear()
    {
        x = new AnimationCurve();
        y = new AnimationCurve();
        z = new AnimationCurve();
        w = new AnimationCurve();
    }
}

/// <summary>
/// ActiveStateInfo
/// </summary>
[System.Serializable]
public class ActiveStateInfo
{
    public bool activeState;
    public float nextChangeTime;

    public ActiveStateInfo(GameObject target, float changeTime)
    {
        activeState = target.activeInHierarchy;
        nextChangeTime = changeTime;
    }
}

/// <summary>
/// ActiveStateInfo
/// </summary>
[System.Serializable]
public class TrailStateInfo
{
    public bool activeState;
    public float nextChangeTime;

    public TrailStateInfo(TrailRenderer target, float changeTime)
    {
        activeState = target.emitting;
        nextChangeTime = changeTime;
    }
}

public class EffectSpawnerStateInfo
{
    public string spawnName;
    public bool isSpawned = false;
    public bool isUseLocal;
    public float spawnTime;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public GameObject spawnParent;
    public GameObject catch_;

    public EffectSpawnerStateInfo(string effectName, float time, Vector3 spawnPosition, GameObject parent, bool isLocal)
    {
        spawnName = effectName;
        spawnTime = time;
        position = spawnPosition;
        spawnParent = parent;
        isUseLocal = isLocal;
    }

    public void Spawn()
    {
        if (isSpawned)
        {
            return;
        }
        isSpawned = true;
        catch_ = EffectSpawnCenter.Instance.FindAndReturenEffectAndSpawnTo(spawnName, position, spawnParent, isUseLocal);
    }

    public void CheckAndClear()
    {
        if (catch_ != null)
        {
            MonoBehaviour.Destroy(catch_);
        }
    }
}

/// <summary>
/// SpawnStateInfo
/// </summary>
[System.Serializable]
public class SpwanStateInfo
{
    public GameObject host;
    public bool isSpawned = false;
    public float spawnTime;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public float autoDestoryIn;

    public SpwanStateInfo(GameObject target, float time, float destoryTime)
    {
        host = MonoBehaviour.Instantiate(target);
        host.SetActive(false);
        spawnTime = time;
        position = target.transform.position;
        rotation = target.transform.rotation;
        scale = target.transform.localScale;
        autoDestoryIn = destoryTime;
    }

    public void Spawn()
    {
        if (isSpawned || host == null)
        {
            return;
        }
        isSpawned = true;
        host.SetActive(true);
        host.transform.position = position;
        host.transform.rotation = rotation;
        host.transform.localScale = scale;
        MonoBehaviour.Destroy(host, autoDestoryIn);
    }

    public void CheckAndClear()
    {
        if (host != null)
        {
            MonoBehaviour.Destroy(host);
        }
    }
}

///Record data
[System.Serializable]
public class RecordData
{
    public TimelineVector position = new TimelineVector();
    public TimeLineQuaternion rotation = new TimeLineQuaternion();
    public TimelineVector scale = new TimelineVector();
    public AllowToRecord allowToRecord;

    [System.Serializable]
    public struct AllowToRecord
    {
        public bool isRecordPos;
        public bool isRecordRot;
        public bool isRecordScale;
        public bool isRecordLocalPos;
        public bool isRecordLocalRot;
    }

    public LastBeforeReplayStates lastState = new LastBeforeReplayStates();

    public class LastBeforeReplayStates
    {
        public bool isKinematic;
        public bool animaeEnable;

        public LastBeforeReplayStates()
        {
            isKinematic = false;
            animaeEnable = false;
        }
    }

    public void Add(Transform t, float currentTime)
    {
        if (allowToRecord.isRecordPos && allowToRecord.isRecordLocalPos == false)
            position.Add(t.position, currentTime);
        if (allowToRecord.isRecordLocalPos && allowToRecord.isRecordPos == false)
            position.Add(t.localPosition, currentTime);
        if (allowToRecord.isRecordRot && allowToRecord.isRecordLocalRot == false)
            rotation.Add(t.rotation, currentTime);
        if (allowToRecord.isRecordLocalRot && allowToRecord.isRecordRot == false)
            rotation.Add(t.localRotation, currentTime);
        if (allowToRecord.isRecordScale)
            scale.Add(t.localScale, currentTime);
    }

    public void Set(float time, Transform transform)
    {
        if (allowToRecord.isRecordPos && allowToRecord.isRecordLocalPos == false)
            transform.position = position.Get(time);

        if (allowToRecord.isRecordLocalPos && allowToRecord.isRecordPos == false)
            transform.localPosition = position.Get(time);
        if (allowToRecord.isRecordRot && allowToRecord.isRecordLocalRot == false)
            transform.rotation = rotation.Get(time);
        if (allowToRecord.isRecordLocalRot && allowToRecord.isRecordRot == false)
            transform.localRotation = rotation.Get(time);
        if (allowToRecord.isRecordScale)
            transform.localScale = scale.Get(time);
    }

    public void ClearData()
    {
        position.Clear();
        rotation.Clear();
        scale.Clear();
    }
}

public class RecordEnitity : MonoBehaviour
{
    public float loopTime;

    [SerializeField]
    public RecordData Data = new RecordData();

    [HideInInspector]
    public Rigidbody rigidbody;

    [HideInInspector]
    public Animator animator;

    public virtual void Start()
    {
        if (RecordManager.Instance)
        {
            loopTime = RecordManager.Instance.loopTime;
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            RecordManager.Instance.OnBeginStart += PerBeginRecord;
            RecordManager.Instance.OnReplayTimeChange += RePlay;
            RecordManager.Instance.OnReplayStart += PerReplayInit;
            RecordManager.Instance.OnEndRecord += EndRecord;
            RecordManager.Instance.OnReplayEnd += ReplayEnd;
            StartCoroutine(Recording());
        }
        else
        {
            enabled = false;
        }
    }

    private IEnumerator Recording()
    {
        if (RecordManager.Instance)
        {
            while (true)
            {
                yield return new WaitForSeconds(1 / RecordManager.Instance.recoredRate);
                if (RecordManager.Instance.isRecording)
                {
                    Record(RecordManager.Instance.GetCurrentTime());
                }
            }
        }
    }

    public virtual void Record(float time)
    {
    }

    public virtual void EndRecord(float time)
    {
    }

    public virtual void PerBeginRecord()
    {
        if (allowRecord() == false)
        {
            return;
        }
        Data.ClearData();
        Data.lastState = new RecordData.LastBeforeReplayStates();
        loopTime = RecordManager.Instance.loopTime;
    }

    public virtual void PerReplayInit()
    {
        if (rigidbody != null)
        {
            Data.lastState.isKinematic = rigidbody.isKinematic;
            rigidbody.isKinematic = true;
        }

        if (animator != null)
        {
            Data.lastState.animaeEnable = animator.enabled;
            animator.enabled = false;
        }
    }

    public virtual void RePlay(float time)
    {
    }

    public virtual void ReplayEnd()
    {
        if (rigidbody != null)
        {
            rigidbody.isKinematic = Data.lastState.isKinematic;
        }

        if (animator != null)
        {
            animator.speed = 1;
            animator.enabled = Data.lastState.animaeEnable;
        }
    }

    public virtual void OnDestroy()
    {
        StopAllCoroutines();
        RecordManager.Instance.OnBeginStart -= PerBeginRecord;
        RecordManager.Instance.OnReplayTimeChange -= RePlay;
        RecordManager.Instance.OnReplayStart -= PerReplayInit;
        RecordManager.Instance.OnEndRecord -= EndRecord;
        RecordManager.Instance.OnReplayEnd -= ReplayEnd;
    }

    bool allowRecord()
    {
        // You can add other control in here to limit Record system
        return true;
    }
}