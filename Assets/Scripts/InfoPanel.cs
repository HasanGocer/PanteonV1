using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoSingleton<InfoPanel>
{
    public enum InfoPanelStat
    {
        free = -1,
        motherbase = 0,
        barracks = 1,
        central = 2,
        miner = 3,
        repairman = 4,
        hospital = 5,
        archer = 6
    }

    [SerializeField] private List<GameObject> _infoPanels = new List<GameObject>();

    private InfoPanelStat _buyInfoPanelStat = InfoPanelStat.free;
    private InfoPanelStat _ShowInfoPanelStat = InfoPanelStat.free;
    [SerializeField] private List<GameObject> _infoButtonPanels = new List<GameObject>();

    [SerializeField] Button _backButton;

    [Header("Buy_And_Upgrade_Buttons")]
    [Space(10)]

    [SerializeField] List<Button> _upgradeButtons = new List<Button>();

    [SerializeField] Button _barracksAddSoliderButton;
    [SerializeField] Button _repairButton;

    [Header("Buy_And_Upgrade_Buttons")]
    [Space(10)]

    [SerializeField] List<TMP_Text> _upgradeTexts = new List<TMP_Text>();

    [SerializeField] TMP_Text _centralPerText;
    [SerializeField] TMP_Text _minerPerText;
    [SerializeField] TMP_Text _repairText;
    [SerializeField] TMP_Text _barracksAddSoliderText;

    [SerializeField] int _repairCost;
    public int soldierCost;

    public void ButtonPlacement()
    {
        _upgradeButtons[(int)InfoPanel.InfoPanelStat.barracks].onClick.AddListener(() => BuildUpgradeButton(InfoPanel.InfoPanelStat.barracks));
        _upgradeButtons[(int)InfoPanel.InfoPanelStat.central].onClick.AddListener(() => BuildUpgradeButton(InfoPanel.InfoPanelStat.central));
        _upgradeButtons[(int)InfoPanel.InfoPanelStat.miner].onClick.AddListener(() => BuildUpgradeButton(InfoPanel.InfoPanelStat.miner));
        _upgradeButtons[(int)InfoPanel.InfoPanelStat.archer].onClick.AddListener(() => BuildUpgradeButton(InfoPanel.InfoPanelStat.archer));

        _repairButton.onClick.AddListener(RepairTime);
        _barracksAddSoliderButton.onClick.AddListener(AddSolider);

        _backButton.onClick.AddListener(BackButton);
    }
    public void TextPlacement()
    {
        _barracksAddSoliderText.text = soldierCost.ToString();
        _repairText.text = _repairCost.ToString();
    }

    public InfoPanelStat GetBuyInfoPanelStat()
    {
        return _buyInfoPanelStat;
    }
    public bool IsBuyInfoPanelStatEmpty()
    {
        if (_buyInfoPanelStat == InfoPanelStat.free) return true;
        else return false;
    }

    public InfoPanelStat OpenBuyInfoPanel(InfoPanelStat tempPanelStat)
    {
        if (_buyInfoPanelStat == InfoPanelStat.free)
        {
            _buyInfoPanelStat = tempPanelStat;
            SetPanel(tempPanelStat, true, false);
            return InfoPanelStat.free;
        }
        else
        {
            InfoPanelStat tempInfoStat = _buyInfoPanelStat;

            SetPanel(_buyInfoPanelStat, false, false);
            _buyInfoPanelStat = tempPanelStat;
            SetPanel(tempPanelStat, true, false);
            return tempInfoStat;
        }
    }
    public void CloseBuyInfoPanel()
    {
        SetPanel(_buyInfoPanelStat, false, false);
        _buyInfoPanelStat = InfoPanelStat.free;
    }

    public InfoPanelStat GetShowInfoPanelStat()
    {
        return _ShowInfoPanelStat;
    }

    public InfoPanelStat OpenShowInfoPanel(InfoPanelStat tempPanelStat)
    {
        if (_ShowInfoPanelStat == InfoPanelStat.free)
        {
            _ShowInfoPanelStat = tempPanelStat;
            SetPanel(tempPanelStat, true, true);
            ToggleBackButtonVisibility(true);
            ButtonTextPlacement(tempPanelStat);
            return InfoPanelStat.free;
        }
        else
        {
            InfoPanelStat tempInfoStat = _ShowInfoPanelStat;

            SetPanel(_ShowInfoPanelStat, false, false);
            _ShowInfoPanelStat = tempPanelStat;
            SetPanel(tempPanelStat, true, true);
            ButtonTextPlacement(tempPanelStat);
            return tempInfoStat;
        }
    }
    public void SetMinerPerText(float perGem)
    {
        _minerPerText.text = "Saniye baþý " + perGem;
    }
    public void SetCentralPerText(float energyGem)
    {
        _minerPerText.text = "Saniye baþý " + energyGem;
    }

    public void CloseShowInfoPanel()
    {
        SetPanel(_ShowInfoPanelStat, false, false);
        ToggleBackButtonVisibility(false);
        _ShowInfoPanelStat = InfoPanelStat.free;
    }

    private void ToggleBackButtonVisibility(bool isOpen)
    {
        _backButton.gameObject.SetActive(isOpen);
    }

    private void SetPanel(InfoPanelStat tempPanelStat, bool isActive, bool isShowPanel)
    {
        _infoPanels[(int)tempPanelStat].SetActive(isActive);
        ToggleBackButtonVisibility(isShowPanel);
        _infoButtonPanels[(int)tempPanelStat].SetActive(isShowPanel);
    }
    private void ButtonTextPlacement(InfoPanelStat buildStat)
    {
        GameObject build = BuildManager.Instance.GetMainBuildTouch().gameObject;

        _upgradeTexts[(int)buildStat].text = BuildManager.Instance.GetBuildData().buildMainDatas[(int)buildStat].Costs[build.GetComponent<InGameSelectedSystem>().GetLevel() - 1].ToString();
        if (buildStat == InfoPanelStat.miner) SetCentralPerText(build.GetComponent<MinerID>().GetPerGem());
        else if (buildStat == InfoPanelStat.central) SetCentralPerText(build.GetComponent<CentralID>().GetPerEnergy());
    }
    private void BackButton()
    {
        CloseShowInfoPanel();
    }

    private void BuildUpgradeButton(InfoPanelStat buildStat)
    {
        MainBuildTouch tempMainBuildTouch = BuildManager.Instance.GetMainBuildTouch();
        GameObject mainBuild = tempMainBuildTouch.gameObject;
        InGameSelectedSystem inGameSelectedSystem = GetComponent<InGameSelectedSystem>();

        inGameSelectedSystem.UpgradeTime(_upgradeTexts[(int)buildStat], buildStat);
    }

    private void AddSolider()
    {
        if (GameManager.Instance.power >= soldierCost)
        {
            MainBuildTouch tempMainBuildTouch = BuildManager.Instance.GetMainBuildTouch();
            GameObject mainBuild = tempMainBuildTouch.gameObject;
            BarracksID barracksID = mainBuild.GetComponent<BarracksID>();

            MoneySystem.Instance.PowerTextRevork(-soldierCost);
            barracksID.AddSolider();
        }
    }
    private void RepairTime()
    {

        if (SelectSystem.Instance.GetSelectEnumStat() == SelectSystem.SelectEnumStat.free)
            SelectSystem.Instance.SelectRepairTime();
        else
            SelectSystem.Instance.SelectFree();
    }
}
