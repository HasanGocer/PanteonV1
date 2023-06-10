using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinerID : MonoBehaviour
{
    [Header("Standart_Field")]
    [Space(10)]

    [SerializeField] MinerData _minerData;
    [SerializeField] MinerTime _minerTime;
    [SerializeField] InGameSelectedSystem inGameSelectedSystem;
    [SerializeField] List<GameObject> _upgrades = new List<GameObject>();

    [Header("Data_Field")]
    [Space(10)]

    [SerializeField] Image _barImage;
    bool isCrash;

    public void StartDataPlacement(int level)
    {
        _upgrades[level-1].SetActive(true);
        inGameSelectedSystem.SetHealth(_minerData.HP[inGameSelectedSystem.GetLevel() - 1]);
        _minerTime.IsStart();
    }

    public void HPDown(int downCount)
    {
        inGameSelectedSystem.SetHealth(inGameSelectedSystem.GetHealth() - downCount);
    }

    public void RepairHP()
    {
        SelectSystem.Instance.SelectFree();
        inGameSelectedSystem.SetHealth(_minerData.HP[inGameSelectedSystem.GetLevel() - 1]);
    }

    public void UpgradeTime(TMP_Text perCountText, TMP_Text upgradeCostText)
    {
        if (inGameSelectedSystem.GetLevel() - 1 < _minerData.HP.Count)
            if (GameManager.Instance.money >= _minerData.Cost[inGameSelectedSystem.GetLevel() - 1])
            {
                BuildVisibility(inGameSelectedSystem.GetLevel(), false);
                MoneySystem.Instance.MoneyTextRevork(-_minerData.Cost[inGameSelectedSystem.GetLevel() - 1]);
                InfoPanel.Instance.OffShowInfoPanel();
                inGameSelectedSystem.UpLevel();
                UpdateLevel();
                CostTextPlacement(perCountText,upgradeCostText);
                SetHP();
                BuildVisibility(inGameSelectedSystem.GetLevel(), true);
            }
    }
    public void CostTextPlacement(TMP_Text perCountText, TMP_Text upgradeCostText)
    {
        perCountText.text = "Saniye baþý " + _minerData.PerGem[inGameSelectedSystem.GetLevel() - 1];

        if (inGameSelectedSystem.GetLevel() == 3)
            upgradeCostText.text = "Full";
        else
            upgradeCostText.text = _minerData.Cost[inGameSelectedSystem.GetLevel() - 1].ToString();
    }
    public void MinerTime()
    {
        MoneySystem.Instance.MoneyTextRevork(_minerData.PerGem[inGameSelectedSystem.GetLevel() - 1]);
    }

    public void Update()
    {
        if (inGameSelectedSystem.GetIsPlacement() && CheckBar((float)inGameSelectedSystem.GetHealth() / (float)_minerData.HP[inGameSelectedSystem.GetLevel() - 1]))
            BarUpdate((float)inGameSelectedSystem.GetHealth() / (float)_minerData.HP[inGameSelectedSystem.GetLevel() - 1]);

        if (!isCrash && inGameSelectedSystem.GetIsPlacement() && inGameSelectedSystem.GetHealth() <= 0) BreakTime();
    }
    private void BreakTime()
    {
        isCrash = true;
        SoundSystem.Instance.CallBuildAbandoned();
        ParticalManager.Instance.CallBuildPartical(gameObject);
        _upgrades[inGameSelectedSystem.GetLevel() - 1].SetActive(false);
        SetBar();
        _minerTime.IsFinish();
        BuildManager.Instance.DeleteBuild(gameObject);
        gameObject.SetActive(false);
    }
    private void SetBar()
    {
        _barImage.fillAmount = 1;
    }
    private void UpdateLevel()
    {
        GridSystem gridSystem = GridSystem.Instance;

        for (int i = 0; i < gridSystem.mainGrid.builds.Count; i++)
            if (gameObject == gridSystem.mainGrid.builds[i])
                gridSystem.mainGrid.buildLevel[i]++;

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

    private void SetHP()
    {
        inGameSelectedSystem.SetHealth(_minerData.HP[inGameSelectedSystem.GetLevel() - 1]);
    }
    private void BuildVisibility(int level, bool isOpen)
    {
        _upgrades[level].SetActive(isOpen);
    }
}
