using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoreLoopSystem : MonoSingleton<CoreLoopSystem>
{
    [SerializeField] Image _bar;

    [SerializeField] float _buildMovementDuration = 0.01f;

    public IEnumerator BarUpdate()
    {
        _bar.fillAmount = 1;
        _bar.DOFillAmount(0, _buildMovementDuration);
        yield return new WaitForSeconds(_buildMovementDuration);
        SoundSystem.Instance.CallWarTime();
        WaveTime();
    }

    public void WaveTime()
    {
        RivalSpawnSystem.Instance.StartEnemyWave();
    }

}
