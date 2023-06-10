using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralTime : MonoBehaviour
{
    [SerializeField] CentralID _centralID;

    bool isStart;

    float _timer;

    public void IsStart()
    {
        isStart = true;
    }
    public void IsFinish()
    {
        isStart = false;
    }

    private void Update()
    {
        if (GameManager.Instance.gameStat == GameManager.GameStat.start && isStart)
        {
            _timer += Time.deltaTime;

            if (_timer >= 1)
            {
                _centralID.CentralTime();
                _timer = 0;
            }
        }
    }
}
