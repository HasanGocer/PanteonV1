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

    [SerializeField] Button _barracksUpgradeButton;
    [SerializeField] Button _barracksAddSoliderButton;
    [SerializeField] Button _centralUpgradeButton;
    [SerializeField] Button _minerUpgradeButton;
    [SerializeField] Button _repairButton;
    [SerializeField] Button _archerUpgradeButton;

    [Header("Buy_And_Upgrade_Buttons")]
    [Space(10)]

    [SerializeField] TMP_Text _barracksUpgradeText;
    [SerializeField] TMP_Text _barracksAddSoliderText;
    [SerializeField] TMP_Text _centralUpgradeText;
    [SerializeField] TMP_Text _centralPerText;
    [SerializeField] TMP_Text _minerUpgradeText;
    [SerializeField] TMP_Text _minerPerText;
    [SerializeField] TMP_Text _repairText;
    [SerializeField] TMP_Text _archerUpgradeText;

    [SerializeField] int _repairCost;
    public int soldierCost;

    public void ButtonPlacement()
    {
        _barracksUpgradeButton.onClick.AddListener(BarracksUpgradeButton);
        _centralUpgradeButton.onClick.AddListener(CentralUpgradeButton);
        _minerUpgradeButton.onClick.AddListener(MinerUpgradeButton);
        _archerUpgradeButton.onClick.AddListener(ArcherUpgradeButton);

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

    public InfoPanelStat OpenShowInfoPanel(GameObject build, InfoPanelStat tempPanelStat)
    {
        if (_ShowInfoPanelStat == InfoPanelStat.free)
        {
            _ShowInfoPanelStat = tempPanelStat;
            SetPanel(tempPanelStat, true, true);
            ToggleBackButtonVisibility(true);
            ButtonTextPlecement(build, tempPanelStat);
            return InfoPanelStat.free;
        }
        else
        {
            InfoPanelStat tempInfoStat = _ShowInfoPanelStat;

            SetPanel(_ShowInfoPanelStat, false, false);
            _ShowInfoPanelStat = tempPanelStat;
            SetPanel(tempPanelStat, true, true);
            ButtonTextPlecement(build, tempPanelStat);
            return tempInfoStat;
        }
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
    private void ButtonTextPlecement(GameObject build, InfoPanelStat tempPanelStat)
    {
        if (tempPanelStat == InfoPanelStat.barracks)
            build.GetComponent<BarracksID>().CostTextPlacement(_barracksUpgradeText);
        else if (tempPanelStat == InfoPanelStat.archer)
            build.GetComponent<ArcherID>().CostTextPlacement(_archerUpgradeText);
        else if (tempPanelStat == InfoPanelStat.miner)
            build.GetComponent<MinerID>().CostTextPlacement(_minerPerText, _minerUpgradeText);
        else if (tempPanelStat == InfoPanelStat.central)
            build.GetComponent<CentralID>().CostTextPlacement(_centralPerText, _centralUpgradeText);
    }
    private void BackButton()
    {
        CloseShowInfoPanel();
    }

    private void BarracksUpgradeButton()
    {
        MainBuildTouch tempMainBuildTouch = BuildManager.Instance.GetMainBuildTouch();
        GameObject mainBuild = tempMainBuildTouch.gameObject;
        BarracksID barracksID = mainBuild.GetComponent<BarracksID>();

        barracksID.UpgradeTime(_barracksUpgradeText);
    }
    private void CentralUpgradeButton()
    {
        MainBuildTouch tempMainBuildTouch = BuildManager.Instance.GetMainBuildTouch();
        GameObject mainBuild = tempMainBuildTouch.gameObject;
        CentralID centralID = mainBuild.GetComponent<CentralID>();

        centralID.UpgradeTime(_centralPerText, _centralUpgradeText);
    }
    private void MinerUpgradeButton()
    {
        MainBuildTouch tempMainBuildTouch = BuildManager.Instance.GetMainBuildTouch();
        GameObject mainBuild = tempMainBuildTouch.gameObject;
        MinerID minerID = mainBuild.GetComponent<MinerID>();

        minerID.UpgradeTime(_minerPerText, _minerUpgradeText);
    }
    private void ArcherUpgradeButton()
    {
        MainBuildTouch tempMainBuildTouch = BuildManager.Instance.GetMainBuildTouch();
        GameObject mainBuild = tempMainBuildTouch.gameObject;
        ArcherID archerID = mainBuild.GetComponent<ArcherID>();

        archerID.UpgradeTime(_archerUpgradeText);
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
