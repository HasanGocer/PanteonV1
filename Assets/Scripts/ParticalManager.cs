using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalManager : MonoSingleton<ParticalManager>
{
    [SerializeField] int _OPMoneyParticalCount;
    [SerializeField] int _OPBuildParticalCount;
    [SerializeField] int _OPHealthParticalCount;
    [SerializeField] int _OPBuildRestoredParticalCount;
    [SerializeField] int _OPBloodParticalCount;

    [SerializeField] int _MoneyParticalTimeCount;
    [SerializeField] int _BuildParticalTimeCount;
    [SerializeField] int _HealthParticalTimeCount;
    [SerializeField] int _BuildRestoredParticalTimeCount;
    [SerializeField] int _BloodParticalTimeCount;

    public void CallMoneyPartical(GameObject pos)
    {
        StartCoroutine(CallMoneyParticalEnum(pos));
    }
    public void CallBuildPartical(GameObject pos)
    {
        StartCoroutine(CallBuildParticalEnum(pos));
    }
    public void CallHealthPartical(GameObject pos)
    {
        StartCoroutine(CallHealthParticalEnum(pos));
    }
    public void CallBuildRestoredPartical(GameObject pos)
    {
        StartCoroutine(CallBuildRestoredParticalEnum(pos));
    }
    public void CallBloodPartical(GameObject pos)
    {
        StartCoroutine(CallBloodParticalEnum(pos));
    }


    private IEnumerator CallMoneyParticalEnum(GameObject pos)
    {
        GameObject partical = ObjectPool.Instance.GetPooledObjectAdd(_OPMoneyParticalCount, pos.transform.position);
        yield return new WaitForSeconds(_MoneyParticalTimeCount);
        partical.SetActive(false);
    }
    private IEnumerator CallBuildParticalEnum(GameObject pos)
    {
        GameObject partical = ObjectPool.Instance.GetPooledObjectAdd(_OPBuildParticalCount, pos.transform.position);
        yield return new WaitForSeconds(_BuildParticalTimeCount);
        partical.SetActive(false);
    }
    private IEnumerator CallHealthParticalEnum(GameObject pos)
    {
        GameObject partical = ObjectPool.Instance.GetPooledObjectAdd(_OPHealthParticalCount, pos.transform.position);
        yield return new WaitForSeconds(_HealthParticalTimeCount);
        partical.SetActive(false);
    }
    private IEnumerator CallBuildRestoredParticalEnum(GameObject pos)
    {
        GameObject partical = ObjectPool.Instance.GetPooledObjectAdd(_OPBuildRestoredParticalCount, pos.transform.position);
        yield return new WaitForSeconds(_BuildRestoredParticalTimeCount);
        partical.SetActive(false);
    }
    private IEnumerator CallBloodParticalEnum(GameObject pos)
    {
        GameObject partical = ObjectPool.Instance.GetPooledObjectAdd(_OPBloodParticalCount, pos.transform.position);
        yield return new WaitForSeconds(_BloodParticalTimeCount);
        partical.SetActive(false);
    }
}
