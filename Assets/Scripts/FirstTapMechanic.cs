using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTapMechanic : MonoSingleton<FirstTapMechanic>
{
    [SerializeField] GameObject hintPanel;
    public bool isMotherBase;

    public void FTStart()
    {
        if (!Load.Instance.isReturn) PanelOn();
        else if (!BuildCheck()) PanelOn();
        else PanelOff();
    }
    private bool BuildCheck()
    {
        GridSystem gridSystem = GridSystem.Instance;

        isMotherBase = false;

        for (int i = 0; i < gridSystem.mainGrid.buildTypes.Count; i++)
        {
            if (gridSystem.mainGrid.buildTypes[i] == InfoPanel.InfoPanelStat.motherbase) isMotherBase = true;
        }

        return isMotherBase;
    }

    private void PanelOn()
    {
        Load.Instance.isReturn = true;
        hintPanel.SetActive(true);
    }
    private void PanelOff()
    {
        hintPanel.SetActive(false);
    }
}
