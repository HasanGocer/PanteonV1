using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarracksID : MonoBehaviour
{
    [Header("Standart_Field")]
    [Space(10)]

    [SerializeField] BarracksData _barracksData;
    [SerializeField] InGameSelectedSystem inGameSelectedSystem;
    [SerializeField] HitTime _hitTime;
    [SerializeField] List<GameObject> _upgrades = new List<GameObject>();

    [Header("Data_Field")]
    [Space(10)]

    [SerializeField] Image _barImage;
    [SerializeField] GameObject _soliderStartPos;

    bool isCrash;

    public void StartDataPlacement(bool isNew, int level)
    {
        if (isNew)
        {
            GridSystem.Instance.mainGrid.buildHP.Add(_barracksData.HPs[0]);
            inGameSelectedSystem.SetHealth(_barracksData.HPs[0]);
        }
        else
            inGameSelectedSystem.SetHealth(GridSystem.Instance.mainGrid.buildHP[GridSystem.Instance.mainGrid.builds.Count - 1]); ;

        _upgrades[level - 1].SetActive(true);
        _hitTime.SetData(_barracksData.countDowns[inGameSelectedSystem.GetLevel() - 1], _barracksData.hitSpeeds[inGameSelectedSystem.GetLevel() - 1], _barracksData.damages[inGameSelectedSystem.GetLevel() - 1], gameObject);
    }

    public void HPDown(int downCount)
    {
        inGameSelectedSystem.SetHealth(inGameSelectedSystem.GetHealth() - downCount);
        SaveHP();
    }

    public void RepairHP()
    {
        inGameSelectedSystem.SetHealth(_barracksData.HPs[inGameSelectedSystem.GetLevel() - 1]);
        SelectSystem.Instance.SelectFree();
    }
    public void CostTextPlacement(TMP_Text upgradeCostText)
    {
        if (inGameSelectedSystem.GetLevel() == 3)
            upgradeCostText.text = "Full";
        else
            upgradeCostText.text = _barracksData.Costs[inGameSelectedSystem.GetLevel() - 1].ToString();
    }

    public void UpgradeTime(TMP_Text upgradeCostText)
    {
        if (inGameSelectedSystem.GetLevel() < _barracksData.HPs.Count)
            if (GameManager.Instance.money >= _barracksData.Costs[inGameSelectedSystem.GetLevel() - 1])
            {
                BuildVisibility(inGameSelectedSystem.GetLevel() - 1, false);
                MoneySystem.Instance.MoneyTextRevork(-_barracksData.Costs[inGameSelectedSystem.GetLevel() - 1]);
                InfoPanel.Instance.OffShowInfoPanel();
                inGameSelectedSystem.UpLevel();
                UpdateLevel();
                CostTextPlacement(upgradeCostText);
                _hitTime.SetData(_barracksData.countDowns[inGameSelectedSystem.GetLevel() - 1], _barracksData.hitSpeeds[inGameSelectedSystem.GetLevel() - 1], _barracksData.damages[inGameSelectedSystem.GetLevel() - 1], gameObject);
                SetHP();
                BuildVisibility(inGameSelectedSystem.GetLevel() - 1, true);
            }
    }

    public void AddSolider()
    {
        GameObject solider = SoliderSpawnSystem.Instance.GetSolider(inGameSelectedSystem.GetLevel() - 1, _soliderStartPos.transform.position);
        solider.GetComponent<SoldierID>().StatPlacement(inGameSelectedSystem.GetLevel());
    }

    public void Update()
    {
        if (inGameSelectedSystem.GetIsPlacement() && CheckBar((float)inGameSelectedSystem.GetHealth() / (float)_barracksData.HPs[inGameSelectedSystem.GetLevel() - 1]))
            BarUpdate((float)inGameSelectedSystem.GetHealth() / (float)_barracksData.HPs[inGameSelectedSystem.GetLevel() - 1]);

        if (!isCrash && inGameSelectedSystem.GetIsPlacement() && inGameSelectedSystem.GetHealth() <= 0) BreakTime();
    }
    private void BreakTime()
    {
        isCrash = true;
        SoundSystem.Instance.CallBuildAbandoned();
        ParticalManager.Instance.CallBuildPartical(gameObject);
        _upgrades[inGameSelectedSystem.GetLevel() - 1].SetActive(false);
        _hitTime.HitTimeOff();
        SetBar();
        BuildManager.Instance.DeleteBuild(gameObject);
        GameManager.Instance.GridPlacementWrite(GridSystem.Instance.mainGrid);
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
        inGameSelectedSystem.SetHealth(_barracksData.HPs[inGameSelectedSystem.GetLevel() - 1]);
    }
    private void BuildVisibility(int level, bool isOpen)
    {
        _upgrades[level].SetActive(isOpen);
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
