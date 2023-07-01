using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class InGameSelectedSystem : MonoBehaviour
{
    public UnityAction startFunc;
    public UnityAction upgradeFunc;
    public InfoPanel.InfoPanelStat mainBuildStat;
    [SerializeField] GameObject _mainBuild;
    [SerializeField] bool isPlacement;

    [SerializeField] int _currentHP = 0;
    [SerializeField] int _level = 1;

    public int GetHealth()
    {
        return _currentHP;
    }
    public void SetHealth(int HP)
    {
        _currentHP = HP;
    }
    public int GetLevel()
    {
        return _level;
    }
    public void SetLevel(int level)
    {
        _level = level;
    }

    public void BuildPlacement()
    {
        isPlacement = true;
    }
    public bool GetIsPlacement()
    {
        return isPlacement;
    }

    public void UpgradeTime(TMP_Text upgradeCostText, InfoPanel.InfoPanelStat buildType)
    {
        BuildData buildData = BuildManager.Instance.GetBuildData();

        if (_level - 1 < buildData.buildMainDatas[(int)buildType].HPs.Count)
            if (GameManager.Instance.money >= buildData.buildMainDatas[(int)buildType].Costs[_level - 1])
            {
                MoneySystem.Instance.MoneyTextRevork(-buildData.buildMainDatas[(int)buildType].Costs[_level - 1]);
                InfoPanel.Instance.CloseShowInfoPanel();
                SetLevel(_level + 1);
                CostTextPlacement(upgradeCostText, buildType);
                upgradeFunc();
            }
    }
    public void CostTextPlacement(TMP_Text upgradeCostText, InfoPanel.InfoPanelStat buildType)
    {
        BuildData buildData = BuildManager.Instance.GetBuildData();

        print(_level);

        if (_level == 3)
            upgradeCostText.text = "Full";
        else
            upgradeCostText.text = buildData.buildMainDatas[(int)buildType].Costs[_level - 1].ToString();
    }

    private void OnMouseDown()
    {
        if (isPlacement)
            MainButton();
    }

    private void MainButton()
    {
        InfoPanel infoPanel = InfoPanel.Instance;

        if (SelectSystem.Instance.GetSelectEnumStat() == SelectSystem.SelectEnumStat.Repair)
            RepairTime(BuildManager.Instance.GetMainBuildTouch().gameObject.GetComponent<RepairmanID>().GetRepairCost());
        else if (infoPanel.IsBuyInfoPanelStatEmpty())
        {
            BuildManager.Instance.SetMainBuildTouch(GetComponent<MainBuildTouch>());
            infoPanel.OpenShowInfoPanel(mainBuildStat);
        }
    }
    private void RepairTime(int repairCost)
    {
        if (repairCost < GameManager.Instance.money)
        {
            InGameSelectedSystem inGameSelectedSystem = _mainBuild.GetComponent<InGameSelectedSystem>();

            MoneySystem.Instance.MoneyTextRevork(-repairCost);

            inGameSelectedSystem.SetHealth(BuildManager.Instance.GetBuildData().buildMainDatas[(int)mainBuildStat].HPs[inGameSelectedSystem.GetLevel() - 1]);
            ParticalManager.Instance.CallBuildRestoredPartical(gameObject);
            SelectSystem.Instance.SelectFree();
        }
    }
}
