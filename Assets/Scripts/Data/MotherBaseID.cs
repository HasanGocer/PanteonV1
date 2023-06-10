using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherBaseID : MonoBehaviour
{
    [Header("Standart_Field")]
    [Space(10)]

    [SerializeField] MotherBaseData _motherBaseData;
    [SerializeField] InGameSelectedSystem inGameSelectedSystem;

    [Header("Data_Field")]
    [Space(10)]

    [SerializeField] Image _barImage;
    bool isCrash;

    public void StartDataPlacement()
    {
        StartCoroutine(CoreLoopSystem.Instance.BarUpdate());
        inGameSelectedSystem.SetHealth(_motherBaseData.HP);
    }

    public void HPDown(int downCount)
    {
        inGameSelectedSystem.SetHealth(inGameSelectedSystem.GetHealth() - downCount);
    }

    public void RepairHP()
    {
        SelectSystem.Instance.SelectFree();
        inGameSelectedSystem.SetHealth(_motherBaseData.HP);
    }

    private void Update()
    {
        if (inGameSelectedSystem.GetIsPlacement() && CheckBar((float)inGameSelectedSystem.GetHealth() / (float)_motherBaseData.HP))
            BarUpdate((float)inGameSelectedSystem.GetHealth() / (float)_motherBaseData.HP);

        if (!isCrash && inGameSelectedSystem.GetIsPlacement() && inGameSelectedSystem.GetHealth() <= 0) BreakTime();
    }
    private void BreakTime()
    {
        isCrash = true;
        SoundSystem.Instance.CallBuildAbandoned();
        ParticalManager.Instance.CallBuildPartical(gameObject);
        FailSystem.Instance.FailTime();
    }

    private bool CheckBar(float rateHP)
    {
        if (rateHP == _barImage.fillAmount) return false;
        else return true;
    }
    private void BarUpdate(float rateHP)
    {
        _barImage.fillAmount = Mathf.Lerp(_barImage.fillAmount, rateHP, Time.deltaTime);
    }
}
