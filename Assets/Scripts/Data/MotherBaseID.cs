using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherBaseID : MonoBehaviour
{
    [Header("Standart_Field")]
    [Space(10)]

    [SerializeField] BuildData _buildData;
    [SerializeField] MotherBaseData _motherBaseData;
    [SerializeField] InGameSelectedSystem inGameSelectedSystem;

    [Header("Data_Field")]
    [Space(10)]

    [SerializeField] Image _barImage;
    [SerializeField] InfoPanel.InfoPanelStat _buildType;
    bool isCrash;

    private void Awake()
    {
        inGameSelectedSystem.startFunc = StartDataPlacement;
        inGameSelectedSystem.upgradeFunc = UpgradeTime;
    }

    public void StartDataPlacement()
    {
        StartCoroutine(CoreLoopSystem.Instance.BarUpdate());
        inGameSelectedSystem.SetLevel(inGameSelectedSystem.GetLevel());
    }

    private void Update()
    {
        if (inGameSelectedSystem.GetIsPlacement() && CheckBar((float)inGameSelectedSystem.GetHealth() / (float)_buildData.buildMainDatas[(int)_buildType].HPs[0]))
            BarUpdate((float)inGameSelectedSystem.GetHealth() / (float)_buildData.buildMainDatas[(int)_buildType].HPs[0]);

        if (!isCrash && inGameSelectedSystem.GetIsPlacement() && inGameSelectedSystem.GetHealth() <= 0) BreakTime();
    }

    private void UpgradeTime()
    {
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
}
