using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HospitalID : MonoBehaviour
{
    [Header("Standart_Field")]
    [Space(10)]

    [SerializeField] HospitalData _hospitalData;
    [SerializeField] InGameSelectedSystem inGameSelectedSystem;

    [Header("Data_Field")]
    [Space(10)]

    [SerializeField] Image _barImage;
    bool isCrash;

    public void StartDataPlacement()
    {
        inGameSelectedSystem.SetHealth(_hospitalData.HP);
    }

    public void HPDown(int downCount)
    {
        inGameSelectedSystem.SetHealth(inGameSelectedSystem.GetHealth() - downCount);
    }

    public void RepairHP()
    {
        SelectSystem.Instance.SelectFree();
        inGameSelectedSystem.SetHealth(_hospitalData.HP);
    }

    private void Update()
    {
        if ( inGameSelectedSystem.GetIsPlacement() && CheckBar((float)inGameSelectedSystem.GetHealth() / (float)_hospitalData.HP))
            BarUpdate((float)inGameSelectedSystem.GetHealth() / (float)_hospitalData.HP);

        if (!isCrash && inGameSelectedSystem.GetIsPlacement() && inGameSelectedSystem.GetHealth() <= 0) BreakTime();
        if (inGameSelectedSystem.GetHealth() <= 0) BreakTime();
    }
    private void BreakTime()
    {
        isCrash = true;
        SoundSystem.Instance.CallBuildAbandoned();
        ParticalManager.Instance.CallBuildPartical(gameObject);
        SetBar();
        BuildManager.Instance.DeleteBuild(gameObject);
        gameObject.SetActive(false);
    }
    private void SetBar()
    {
        _barImage.fillAmount = 1;
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
