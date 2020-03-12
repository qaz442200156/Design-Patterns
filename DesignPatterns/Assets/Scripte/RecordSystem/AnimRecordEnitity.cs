using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimRecordEnitity : RecordEnitity
{
    public AnimatorControllerParameter animatorController;
    public AnimationCurve animTime = new AnimationCurve();
    public List<animNameInfo> animName = new List<animNameInfo>();
    public List<int> allStateNams = new List<int>();

    public class animNameInfo
    {
        public int name;//NameHash
        public float changeTime;
        public float crossFadeDuration = 0;
        public float startCrossFadeTime = -1;
        public bool isDoneCrossFade;
        public animNameInfo(int newName)
        {
            name = newName;
            changeTime = -1;
        }
    }

    public int lastAnimNameHash;
    public float lastAnimNameStateLength;
    public int StateIndex;
    public float lastTransitionDuration;

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
        animTime.AddKey(RecordManager.Instance.GetCurrentTime(), animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        lastAnimNameHash = getCurrentName();
        animName.Add(new animNameInfo(lastAnimNameHash));
    }

    public override void Record(float time)
    {
        float playedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float clipLength = animator.GetCurrentAnimatorStateInfo(0).length;
        int currentState = getCurrentName();
        if (playedTime > 0)
        {
            animTime.AddKey(time, playedTime);
        }
        if (currentState != 0)
        {
            float animatorTransitionInfo = animator.GetAnimatorTransitionInfo(0).duration;
            if (animatorTransitionInfo > 0)
            {
                Data.Add(transform, time);
                lastTransitionDuration = animatorTransitionInfo;
            }

            if (lastAnimNameHash != currentState)
            {
                if (animName.Count > 0)
                {
                    animName.Last().changeTime = time;
                }
                Debug.Log(">>>>> " + lastTransitionDuration);
                animNameInfo tmp = new animNameInfo(currentState);
                tmp.crossFadeDuration = lastTransitionDuration;
                lastAnimNameHash = currentState;
                lastTransitionDuration = 0;
                animName.Add(tmp);
            }
        }
    }

    public override void PerReplayInit()
    {
        base.PerReplayInit();
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

    public override void RePlay(float time)
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
                animName[StateIndex].startCrossFadeTime = -1;
                StateIndex++;
                currentAnimInfo = animName[StateIndex];
            }
        }

        if (!currentAnimInfo.isDoneCrossFade && StateIndex > 0) {
            animator.enabled = false;
            if (currentAnimInfo.startCrossFadeTime == -1)
            {
                currentAnimInfo.startCrossFadeTime = time;
            }
            float duration = time - currentAnimInfo.startCrossFadeTime;
            if (duration >= currentAnimInfo.crossFadeDuration)
            {
                currentAnimInfo.isDoneCrossFade = true;
                animator.enabled = true;
            }
            else
            {
                Data.Set(time,transform);
            }
        }
        else { 
            animator.Play( currentAnimInfo.name , 0, animTime.Evaluate(time+ currentAnimInfo.crossFadeDuration));
        }
    }

    public override void ReplayEnd()
    {
        base.ReplayEnd();
        animTime = new AnimationCurve();
        animName = new List<animNameInfo>();
        StateIndex = 0;
    }
}