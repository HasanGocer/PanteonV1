using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameSelectedSystem : MonoBehaviour
{
    public UnityAction startFunc;
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

    private void OnMouseDown()
    {
        if (isPlacement)
        {
            MainButton();
        }
    }

    private void MainButton()
    {
        InfoPanel infoPanel = InfoPanel.Instance;

        if (SelectSystem.Instance.GetSelectEnumStat() == SelectSystem.SelectEnumStat.Repair)
            RepairTime(BuildManager.Instance.GetMainBuildTouch().gameObject.GetComponent<RepairmanID>().GetRepairCost());
        else if (infoPanel.IsBuyInfoPanelStatEmpty())
        {
            BuildManager.Instance.SetMainBuildTouch(GetComponent<MainBuildTouch>());
            infoPanel.OpenShowInfoPanel(gameObject, mainBuildStat);
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
