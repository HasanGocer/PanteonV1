using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem : MonoSingleton<HitSystem>
{
    [SerializeField] int _OPHitCount;

    public GameObject GetHit(Vector3 hitPos)
    {
        return ObjectPool.Instance.GetPooledObjectAdd(_OPHitCount, hitPos);
    }
}
