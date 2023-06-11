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

    public void StartDataPlacement(bool isNew)
    {
        if (isNew)
        {
            GridSystem.Instance.mainGrid.buildHP.Add(_motherBaseData.HP);
            inGameSelectedSystem.SetHealth(_motherBaseData.HP);
        }
        else
            inGameSelectedSystem.SetHealth(GridSystem.Instance.mainGrid.buildHP[GridSystem.Instance.mainGrid.builds.Count - 1]); ;

        StartCoroutine(CoreLoopSystem.Instance.BarUpdate());
    }

    public void HPDown(int downCount)
    {
        inGameSelectedSystem.SetHealth(inGameSelectedSystem.GetHealth() - downCount);
        SaveHP();
    }

    public void RepairHP()
    {
        inGameSelectedSystem.SetHealth(_motherBaseData.HP);
        SelectSystem.Instance.SelectFree();
    }

    private void Update()
    {
        if (inGameSelectedSystem.GetIsPlacement() && CheckBar((float)inGameSelectedSystem.GetHealth() / (float)_motherBaseData.HP))
            BarUpdate((float)inGameSelectedSystem.GetHealth() / (float)_motherBaseData.HP);

        if (!isCrash && inGameSelectedSystem.GetIsPlacement() && inGameSelectedSystem.GetHealth() <= 0) BreakTime();
    }
    private void BreakTime()
    {
        GridSystem.Instance.mainGrid.isFinish = true;
        isCrash = true;
        SoundSystem.Instance.CallBuildAbandoned();
        ParticalManager.Instance.CallBuildPartical(gameObject);
        FailSystem.Instance.FailTime();
        GameManager.Instance.GridPlacementWrite(GridSystem.Instance.mainGrid);
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
    private void SaveHP()
    {
        GridSystem gridSystem = GridSystem.Instance;

        for (int i = 0; i < gridSystem.mainGrid.builds.Count; i++)
            if (gridSystem.mainGrid.builds[i] == gameObject)
            {
                gridSystem.mainGrid.buildHP[i] = inGameSelectedSystem.GetHealth();
            }
    }
}
