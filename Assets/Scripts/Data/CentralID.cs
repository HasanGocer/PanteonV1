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
        inGameSelectedSystem.upgradeFunc = UpgradeTime;
    }

    public void StartDataPlacement()
    {
        _upgrades[inGameSelectedSystem.GetLevel() - 1].SetActive(true);
        _centralTime.IsStart();
        inGameSelectedSystem.SetLevel(inGameSelectedSystem.GetLevel());
    }

    public void CentralTime()
    {
        MoneySystem.Instance.PowerTextRevork(_centralData.PerEnergy[inGameSelectedSystem.GetLevel() - 1]);
    }

    public float GetPerEnergy()
    {
        return _centralData.PerEnergy[inGameSelectedSystem.GetLevel() - 1];
    }

    public void Update()
    {
        if (inGameSelectedSystem.GetIsPlacement() && CheckBar((float)inGameSelectedSystem.GetHealth() / (float)_buildData.buildMainDatas[(int)_buildType].HPs[inGameSelectedSystem.GetLevel() - 1]))
            BarUpdate((float)inGameSelectedSystem.GetHealth() / (float)_buildData.buildMainDatas[(int)_buildType].HPs[inGameSelectedSystem.GetLevel() - 1]);

        if (!isCrash && inGameSelectedSystem.GetIsPlacement() && inGameSelectedSystem.GetHealth() <= 0) BreakTime();
    }

    private void UpgradeTime()
    {
        BuildVisibility(inGameSelectedSystem.GetLevel(), false);
        UpdateLevel();
        InfoPanel.Instance.SetCentralPerText(GetPerEnergy());
        SetHP();
        BuildVisibility(inGameSelectedSystem.GetLevel(), true);
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
}
