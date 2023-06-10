using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSystem : MonoSingleton<FinishSystem>
{
    int _rivalCount, _currentRivalCount;

    //Her dalga i�in dalgadaki toplam rival say�s�n� i�liyor
    public void SetRivalCount(int rivalCount)
    {
        _currentRivalCount = 0;
        _rivalCount = rivalCount;
    }

    //her rival �ld���nde �al���r ve t�m rivallerin �l�p �lmedi�ini kontrol ediyor
    public void FinishCheck()
    {
        _currentRivalCount++;
        if (_rivalCount <= _currentRivalCount)
            FinishTime();
    }

    //dalgay� s�f�rl�yor
    private void FinishTime()
    {
        print(3);
        GameManager gameManager = GameManager.Instance;

        gameManager.SetLevel();
        StartCoroutine(CoreLoopSystem.Instance.BarUpdate());
    }

}
