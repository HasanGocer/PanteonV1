using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSystem : MonoSingleton<FinishSystem>
{
    int _rivalCount, _currentRivalCount;

    //Her dalga için dalgadaki toplam rival sayýsýný iþliyor
    public void SetRivalCount(int rivalCount)
    {
        _currentRivalCount = 0;
        _rivalCount = rivalCount;
    }

    //her rival öldüðünde çalýþýr ve tüm rivallerin ölüp ölmediðini kontrol ediyor
    public void FinishCheck()
    {
        _currentRivalCount++;
        if (_rivalCount <= _currentRivalCount)
            FinishTime();
    }

    //dalgayý sýfýrlýyor
    private void FinishTime()
    {
        print(3);
        GameManager gameManager = GameManager.Instance;

        gameManager.SetLevel();
        StartCoroutine(CoreLoopSystem.Instance.BarUpdate());
    }

}
