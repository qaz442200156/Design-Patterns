using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CurveExtended;

public class AnimRecordEnitity : RecordEnitity
{
    public AnimatorControllerParameter animatorController;
    public AnimationCurve animTime = new AnimationCurve();
    public List<animNameInfo> animName = new List<animNameInfo>();
    public List<int> allStateNams = new List<int>();
    public Dictionary<string, transitionInfo> stateTransitionTime = new Dictionary<string, transitionInfo>();
    public class transitionInfo {
        public float duration;
        public transitionInfo(float transitionDuration)
        {
            duration = transitionDuration;
        }
    }
    public class animNameInfo
    {
        public int name;//NameHash
        public int nextStateName;
        public float changeTime;
        public float crossFadeDuration = 0;
        public float startCrossFadeTime = -1f;

        public bool isStartCrossFade = false;
        public bool isDoneCrossFade = false;
        public animNameInfo(int newName)
        {
            name = newName;
            changeTime = -1;
        }
    }

    public int lastAnimNameHash;
    public float lastAnimNameStateLength;
    public int StateIndex;

    public override void Start()
    {
        base.Start();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private int getCurrentName()
    {
        int nameHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        if (allStateNams.Contains(nameHash) == false)
        {
            allStateNams.Add(nameHash);
        }
        return nameHash;
    }

    public override void PerBeginRecord()
    {
        base.PerBeginRecord();
        animTime = new AnimationCurve();
        animName = new List<animNameInfo>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("animation recording object is null  =>" + this.gameObject.name);
                enabled = false;
            }
        }
        Data.lastState.animaeEnable = animator.enabled;
        animTime.AddKey(createKey(RecordManager.Instance.GetCurrentTime(), animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        );
        lastAnimNameHash = getCurrentName();
        animName.Add(new animNameInfo(lastAnimNameHash));
    }

    public override void Record(float time)
    {
        float playedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float clipLength = animator.GetCurrentAnimatorStateInfo(0).length;
        int currentState = getCurrentName();
        Debug.Log(currentState);
        if (currentState != 0)
        {
            bool isInTransition = animator.IsInTransition(0);
           
            if (isInTransition)
            {
                AnimatorTransitionInfo animatorTransitionInfo = animator.GetAnimatorTransitionInfo(0);
                int nextStateName = animator.GetNextAnimatorStateInfo(0).fullPathHash;

                string key = string.Format("{0}|{1}", currentState, nextStateName);
                if (!stateTransitionTime.Keys.Contains(key))
                {
                    stateTransitionTime.Add(key, new transitionInfo(animatorTransitionInfo.duration));
                }
                if (animName.Count > 0)
                {
                    if (animName.Last().startCrossFadeTime == -1 && nextStateName != 0)
                    {
                        Debug.Log(" >>>>>  " + key);
                        animName.Last().nextStateName = nextStateName;
                        animName.Last().crossFadeDuration = animatorTransitionInfo.duration;
                        animName.Last().startCrossFadeTime = time;
                    }
                }
            }

            if (lastAnimNameHash != currentState)
            {
                animNameInfo tmp = new animNameInfo(currentState);
                if (playedTime > 0)
                {
                    animTime.AddKey(createKey(time, playedTime));
                }
                if (animName.Count > 0)
                {
                    animName.Last().changeTime = time;
                }
                tmp.crossFadeDuration = 0;

                if (lastAnimNameHash != 0)
                {
                    foreach (string key in stateTransitionTime.Keys)
                    {
                        string[] stateNameHash = key.Split('|');
                        if (stateNameHash[0].Contains(lastAnimNameHash.ToString()) && stateNameHash[1].Contains(currentState.ToString()))
                        {
                            tmp.crossFadeDuration = stateTransitionTime[key].duration;
                        }
                    }
                }
                lastAnimNameHash = currentState;
                animName.Add(tmp);
            }
        }
    }

    public override void EndRecord(float time)
    {
        float playedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (playedTime > 0)
        {
            animTime.AddKey(createKey(time, playedTime));
        }
    }

    public override void PerReplayInit()
    {
        base.PerReplayInit();
        animTime.UpdateAllLinearTangents();

        animator.enabled = true;
        animator.speed = 0;
        StateIndex = 0;
        foreach (var data in animName)
        {
            if (data.changeTime < 0)
            {
                break;
            }
            else
            {
                if (data.changeTime < RecordManager.Instance.startTime)
                {
                    StateIndex++;
                }
            }
        }
        if (StateIndex >= animName.Count)
        {
            StateIndex = animName.Count - 1;
        }
    }

    public override void RePlay(float time, float timeScale)
    {
        if (animName == null || StateIndex >= animName.Count || animName.Count == 0)
        {
            Debug.Log(gameObject.name + " || " + animName.Count + " || " + StateIndex);
            return;
        }
        animNameInfo currentAnimInfo = animName[StateIndex];

        if (StateIndex < animName.Count - 1 && time > currentAnimInfo.changeTime)
        {
            if (currentAnimInfo.changeTime > 0 && animName.Count > 1)
            {
                animName[StateIndex].isDoneCrossFade = false;
                currentAnimInfo.isStartCrossFade = false;
                StateIndex++;
                currentAnimInfo = animName[StateIndex];
                Debug.Log("Change To =>"+currentAnimInfo.name);
            }
        }

        if (!currentAnimInfo.isDoneCrossFade && time >= currentAnimInfo.startCrossFadeTime  && currentAnimInfo.nextStateName != 0) {
            if (!currentAnimInfo.isStartCrossFade)
            {
                Debug.Log(currentAnimInfo.name+ " -> "+ currentAnimInfo.nextStateName);
                currentAnimInfo.isStartCrossFade = true;
                animator.CrossFade(currentAnimInfo.nextStateName, currentAnimInfo.crossFadeDuration, 0);
            }
            animator.speed = 1 * timeScale;
            float duration = time - currentAnimInfo.startCrossFadeTime;
            if (duration >= currentAnimInfo.crossFadeDuration)
            {
                animator.speed = 0 * timeScale;
                currentAnimInfo.isDoneCrossFade = true;
            }
            return;
        }
        else {
            animator.Play(currentAnimInfo.name, 0, animTime.Evaluate(time));
        }
    }

    public override void ReplayEnd()
    {
        base.ReplayEnd();
        /*
        animTime = new AnimationCurve();
        animName = new List<animNameInfo>();
        StateIndex = 0;*/
    }

    public Keyframe createKey(float time, float value)
    {
        Keyframe tmp = KeyframeUtil.GetNew(time, value, TangentMode.Smooth, TangentMode.Smooth);
        return tmp;
    }
}