using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CentralID : MonoBehaviour
{
    [Header("Standart_Field")]
    [Space(10)]

    [SerializeField] BuildData _buildData;
    [SerializeField] CentralData _centralData;
    [SerializeField] CentralTime _centralTime;
    [SerializeField] InGameSelectedSystem inGameSelectedSystem;
    [SerializeField] List<GameObject> _upgrades = new List<GameObject>();

    [Header("Data_Field")]
    [Space(10)]

    [SerializeField] Image _barImage;
    [SerializeField] InfoPanel.InfoPanelStat _buildType;
    bool isCrash;

    private void Awake()
    {
        inGameSelectedSystem.startFunc = StartDataPlacement;
    }

    public void StartDataPlacement()
    {
        _upgrades[inGameSelectedSystem.GetLevel() - 1].SetActive(true);
        _centralTime.IsStart();
        inGameSelectedSystem.SetLevel(inGameSelectedSystem.GetLevel());
    }

    public void HPDown(int downCount)
    {
        inGameSelectedSystem.SetHealth(inGameSelectedSystem.GetHealth() - downCount);
        SaveHP();
    }

    public void UpgradeTime(TMP_Text perCountText, TMP_Text upgradeCostText)
    {
        if (inGameSelectedSystem.GetLevel() < _buildData.buildMainDatas[(int)_buildType].HPs.Count)
            if (GameManager.Instance.money >= _buildData.buildMainDatas[(int)_buildType].Costs[inGameSelectedSystem.GetLevel() - 1])
            {
                BuildVisibility(inGameSelectedSystem.GetLevel(), false);
                MoneySystem.Instance.MoneyTextRevork(-_buildData.buildMainDatas[(int)_buildType].Costs[inGameSelectedSystem.GetLevel() - 1]);
                InfoPanel.Instance.CloseShowInfoPanel();
                inGameSelectedSystem.SetLevel(inGameSelectedSystem.GetLevel() + 1);
                CostTextPlacement(perCountText, upgradeCostText);
                UpdateLevel();
                SetHP();
                BuildVisibility(inGameSelectedSystem.GetLevel(), true);
            }
    }
    public void CostTextPlacement(TMP_Text perCountText, TMP_Text upgradeCostText)
    {
        perCountText.text = "Saniye baþý " + _centralData.PerEnergy[inGameSelectedSystem.GetLevel() - 1];

        if (inGameSelectedSystem.GetLevel() == 3)
            upgradeCostText.text = "Full";
        else
            upgradeCostText.text = _buildData.buildMainDatas[(int)_buildType].Costs[inGameSelectedSystem.GetLevel() - 1].ToString();
    }
    public void CentralTime()
    {
        MoneySystem.Instance.PowerTextRevork(_centralData.PerEnergy[inGameSelectedSystem.GetLevel() - 1]);
    }

    public void Update()
    {
        if (inGameSelectedSystem.GetIsPlacement() && CheckBar((float)inGameSelectedSystem.GetHealth() / (float)_buildData.buildMainDatas[(int)_buildType].HPs[inGameSelectedSystem.GetLevel() - 1]))
            BarUpdate((float)inGameSelectedSystem.GetHealth() / (float)_buildData.buildMainDatas[(int)_buildType].HPs[inGameSelectedSystem.GetLevel() - 1]);

        if (!isCrash && inGameSelectedSystem.GetIsPlacement() && inGameSelectedSystem.GetHealth() <= 0) BreakTime();
    }
    private void BreakTime()
    {
        isCrash = true;
        SoundSystem.Instance.CallBuildAbandoned();
        ParticalManager.Instance.CallBuildPartical(gameObject);
        _upgrades[inGameSelectedSystem.GetLevel() - 1].SetActive(false);
        SetBar();
        _centralTime.IsFinish();
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
        inGameSelectedSystem.SetHealth(_buildData.buildMainDatas[(int)_buildType].HPs[inGameSelectedSystem.GetLevel() - 1]);
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
