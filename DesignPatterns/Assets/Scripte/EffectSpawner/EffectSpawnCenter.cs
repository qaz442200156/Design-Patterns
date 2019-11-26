using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawnCenter : MonoBehaviour
{
    public static EffectSpawnCenter Instance;
    public PoolBehaviour[] allGameEffects;
    private Dictionary<string, PoolBehaviour> effectDic_ = new Dictionary<string, PoolBehaviour>();

    public void Awake()
    {
        Instance = this;
        initBuildEffectDictionary();
    }

    /// <summary>
    /// エフェクトリストの初期化
    /// </summary>
    void initBuildEffectDictionary()
    {
        foreach (PoolBehaviour effect in allGameEffects)
        {
            effectDic_.Add(effect.name, effect);
        }
    }

    /// <summary>
    /// 単純エフェクトの生成
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="pos"></param>
    /// <param name="parent"></param>
    /// <param name="useLocal"></param>
    public void FindEffectAndSpawnTo(string effectName, Vector3 pos, GameObject parent = null, bool useLocal = false)
    {
        if (effectDic_.ContainsKey(effectName))
        {
            GameObject tmp = effectDic_[effectName].Create(parent);

            setPosition(tmp, pos, useLocal);

            Destroy(tmp, tryGETEffectDestoryTime(tmp));
            return;
        }
        ShowWarning(effectName);
    }

    /// <summary>
    /// エフェクトが生成した後、エフェクトのオブジェをReturn
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="pos"></param>
    /// <param name="parent"></param>
    /// <param name="useLocal"></param>
    /// <returns></returns>
    public GameObject FindAndReturenEffectAndSpawnTo(string effectName, Vector3 pos, GameObject parent = null, bool useLocal = false)
    {
        if (effectDic_.ContainsKey(effectName))
        {
            GameObject tmp = effectDic_[effectName].Create(parent);

            setPosition(tmp, pos, useLocal);

            Destroy(tmp, tryGETEffectDestoryTime(tmp));

            return tmp;
        }
        ShowWarning(effectName);
        return null;
    }

    /// <summary>
    /// 少し待つで生成する
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="pos"></param>
    /// <param name="parent"></param>
    /// <param name="useLocal"></param>
    /// <param name="delay"></param>
    public void FindEffectAndSpawnToWithDelay(string effectName, Vector3 pos, GameObject parent = null, bool useLocal = false, float delay = 0)
    {
        StartCoroutine(FindEffectAndSpawnToWithDelay(delay, () =>
        {
            FindEffectAndSpawnTo(effectName, pos, parent, useLocal);
        }));
    }

    IEnumerator FindEffectAndSpawnToWithDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    #region 生成処理
    /// <summary>
    /// 生成の座標を指定する
    /// </summary>
    /// <param name="target"></param>
    /// <param name="newPosition"></param>
    /// <param name="useLocal"></param>
    void setPosition(GameObject target, Vector3 newPosition, bool useLocal)
    {
        if (useLocal)
        {
            target.transform.localPosition = newPosition;
        }
        else
        {
            target.transform.position = newPosition;
        }
    }
    
    /// <summary>
    /// エフェクトが消滅する時間を直接Obejctから取得する
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="forceDestoryTime"></param>
    /// <param name="defaultDestoryTime"></param>
    /// <returns></returns>
    float tryGETEffectDestoryTime(GameObject effect, float forceDestoryTime = -1, float defaultDestoryTime = 3)
    {
        if (forceDestoryTime > 0)
        {
            return forceDestoryTime;
        }
        float duration = 0;
        ParticleSystem[] childParticleSystems = effect.GetComponentsInChildren<ParticleSystem>();
        if (childParticleSystems.Length > 0)
        {
            foreach (ParticleSystem particle in effect.GetComponentsInChildren<ParticleSystem>())
            {
                float newDuration = 0;
                newDuration += particle.main.duration;
                newDuration += particle.main.startDelayMultiplier;
                newDuration += particle.main.startLifetimeMultiplier;

                if (newDuration > duration)
                {
                    duration = newDuration;
                }
            }
        }
        else
        {
            foreach (Animator animator in effect.GetComponentsInChildren<Animator>())
            {
                float newDuration = 0;
                newDuration += animator.GetCurrentAnimatorStateInfo(0).length;

                if (newDuration > duration)
                {
                    duration = newDuration;
                }
            }
        }
        if (duration == 0)
        {
            return defaultDestoryTime;
        }
        return duration;
    }
    #endregion

    #region 警告対応
    void ShowWarning(string effectName) {
        Debug.LogWarning("Waring ! " + effectName + " was not exist in effect list");
    }
    #endregion
}
