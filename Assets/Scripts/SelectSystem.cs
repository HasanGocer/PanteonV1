using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSystem : MonoSingleton<SelectSystem>
{
    public enum SelectEnumStat
    {
        free,
        BuildPlacement,
        Repair,
        soldierMove
    }

    [SerializeField] SelectEnumStat _selectEnumStat = SelectEnumStat.free;

    public SelectEnumStat GetSelectEnumStat()
    {
        return _selectEnumStat;
    }

    public void SelectedObjectFree()
    {
        if (_selectEnumStat == SelectEnumStat.soldierMove)
            SoldierMoveSystem.Instance.SoldierFree();
        else if (_selectEnumStat == SelectEnumStat.BuildPlacement)
            MarketPanel.Instance.NewItemSelected(InfoPanel.Instance.GetBuyInfoPanelStat());
        else if (_selectEnumStat == SelectEnumStat.Repair)
            InfoPanel.Instance.OffShowInfoPanel();

        SelectFree();
    }

    public void SelectSoldierMove()
    {
        SelectedObjectFree();
        _selectEnumStat = SelectEnumStat.soldierMove;
    }
    public void SelectRepairTime()
    {
        SelectedObjectFree();
        _selectEnumStat = SelectEnumStat.Repair;
    }
    public void SelectBuildPlacement()
    {
        SelectedObjectFree();
        _selectEnumStat = SelectEnumStat.BuildPlacement;
    }
    public void SelectFree()
    {
        _selectEnumStat = SelectEnumStat.free;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            if (_selectEnumStat == SelectEnumStat.BuildPlacement)
            {
                _selectEnumStat = SelectEnumStat.free;

                BuildManager.Instance.BuildPlacement();
            }
    }
}
