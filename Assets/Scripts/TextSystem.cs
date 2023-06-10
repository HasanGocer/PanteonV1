using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSystem : MonoSingleton<TextSystem>
{
    [SerializeField] int _OPPlusCount;
    [SerializeField] float _plusLifeTime;

    public void PlusTime(GameObject pos)
    {
        StartCoroutine(PlusTimeEnum(pos));
    }

    public IEnumerator PlusTimeEnum(GameObject pos)
    {
        GameObject plus = ObjectPool.Instance.GetPooledObjectAdd(_OPPlusCount, pos.transform.position);
        yield return new WaitForSeconds(_plusLifeTime);
        plus.SetActive(false);
    }

}
