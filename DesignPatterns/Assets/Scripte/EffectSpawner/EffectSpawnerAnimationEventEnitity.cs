using UnityEngine;

public class EffectSpawnerAnimationEventEnitity : MonoBehaviour
{
    [System.Serializable]
    public struct EffectInfo
    {
        public string name;
        public Transform spawnPos;
        public Vector3 posOffset;
    }

    public EffectInfo effectInfo;
    public bool autoGetSelf = false;

    public void Awake()
    {
        if (autoGetSelf)
        {
            effectInfo.spawnPos = this.transform;
        }
    }

    /// <summary>
    /// UnityモーションClip EVENT Action
    /// </summary>
    public void EffectSpawnEnitity()
    {
        if (effectInfo.spawnPos != null)
        {
            EffectSpawnCenter.Instance.FindEffectAndSpawnTo(effectInfo.name, effectInfo.spawnPos.position + effectInfo.spawnPos.TransformDirection(effectInfo.posOffset));
        }
        else
        {
            EffectSpawnCenter.Instance.FindEffectAndSpawnTo(effectInfo.name, effectInfo.posOffset);
        }
    }

}
