using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderSpawnSystem : MonoSingleton<SoliderSpawnSystem>
{
    [SerializeField] int _OPSoliderCount;
    [SerializeField] GameObject _soliderParent;

    public GameObject GetSolider(int level, Vector3 pos)
    {
        return ObjectPool.Instance.GetPooledObjectAdd(_OPSoliderCount, pos, Vector3.zero, _soliderParent.transform);
    }
    public void DeleteSoldier(int level)
    {
        GridSystem.Instance.mainGrid.soldierCount[level - 1]--;
    }
}
