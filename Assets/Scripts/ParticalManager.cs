using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalManager : MonoSingleton<ParticalManager>
{
    [SerializeField] int _OPMoneyParticalCount;
    [SerializeField] int _OPBuildParticalCount;

    [SerializeField] int _OPMoneyParticalTimeCount;
    [SerializeField] int _OPBuildParticalTimeCount;

    public void CallMoneyPartical(GameObject pos)
    {
        StartCoroutine(CallMoneyParticalEnum(pos));
    }
    public void CallBuildPartical(GameObject pos)
    {
        StartCoroutine(CallBuildParticalEnum(pos));
    }


    public IEnumerator CallMoneyParticalEnum(GameObject pos)
    {
        GameObject partical = ObjectPool.Instance.GetPooledObjectAdd(_OPMoneyParticalCount, pos.transform.position);
        yield return new WaitForSeconds(_OPMoneyParticalTimeCount);
        partical.SetActive(false);
    }
    public IEnumerator CallBuildParticalEnum(GameObject pos)
    {
        GameObject partical = ObjectPool.Instance.GetPooledObjectAdd(_OPBuildParticalCount, pos.transform.position);
        yield return new WaitForSeconds(_OPBuildParticalTimeCount);
        partical.SetActive(false);
    }
}
