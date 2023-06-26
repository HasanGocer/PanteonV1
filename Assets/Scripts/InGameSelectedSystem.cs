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
        else if (infoPanel.BuyInfoPanelStatIsFull())
        {
            BuildManager.Instance.SetMainBuildTouch(GetComponent<MainBuildTouch>());
            infoPanel.OnShowInfoPanel(gameObject, mainBuildStat);
        }
    }
    private void RepairTime(int repairCost)
    {
        if (repairCost < GameManager.Instance.money)
        {
            MoneySystem.Instance.MoneyTextRevork(-repairCost);

            if (mainBuildStat == InfoPanel.InfoPanelStat.motherbase) _mainBuild.GetComponent<MotherBaseID>().RepairHP();
            else if (mainBuildStat == InfoPanel.InfoPanelStat.repairman) _mainBuild.GetComponent<RepairmanID>().RepairHP();
            else if (mainBuildStat == InfoPanel.InfoPanelStat.miner) _mainBuild.GetComponent<MinerID>().RepairHP();
            else if (mainBuildStat == InfoPanel.InfoPanelStat.hospital) _mainBuild.GetComponent<HospitalID>().RepairHP();
            else if (mainBuildStat == InfoPanel.InfoPanelStat.central) _mainBuild.GetComponent<CentralID>().RepairHP();
            else if (mainBuildStat == InfoPanel.InfoPanelStat.barracks) _mainBuild.GetComponent<BarracksID>().RepairHP();
            else if (mainBuildStat == InfoPanel.InfoPanelStat.archer) _mainBuild.GetComponent<ArcherID>().RepairHP();
        }
    }
}
