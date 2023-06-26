using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketPanel : MonoSingleton<MarketPanel>
{
    [Header("Buttons")]
    [Space(10)]

    [SerializeField] Button _motherBaseBuyButton;
    [SerializeField] Button _motherBaseCancelButton;
    [SerializeField] Button _barracksBuyButton, _barracksCancelButton;
    [SerializeField] Button _centralBuyButton, _centralCancelButton;
    [SerializeField] Button _minerBuyButton, _minerCancelButton;
    [SerializeField] Button _repairmanBuyButton, _repairmanCancelButton;
    [SerializeField] Button _hospitalBuyButton, _hospitalCancelButton;
    [SerializeField] Button _archerBuyButton, _archerCancelButton;

    [Header("Cost_Text")]
    [Space(10)]

    [SerializeField] TMP_Text _motherBaseCostText;
    [SerializeField] TMP_Text _barracksCostText;
    [SerializeField] TMP_Text _centralCostText;
    [SerializeField] TMP_Text _minerCostText;
    [SerializeField] TMP_Text _repairmanCostText;
    [SerializeField] TMP_Text _hospitalCostText;
    [SerializeField] TMP_Text _archerCostText;

    [Header("Limit_Text")]
    [Space(10)]

    [SerializeField] TMP_Text _motherBaseLimitText;
    [SerializeField] TMP_Text _barracksLimitText;
    [SerializeField] TMP_Text _centralLimitText;
    [SerializeField] TMP_Text _minerLimitText;
    [SerializeField] TMP_Text _repairmanLimitText;
    [SerializeField] TMP_Text _hospitalLimitText;
    [SerializeField] TMP_Text _archerLimitText;

    [Header("Build_Field")]
    [Space(10)]

    [SerializeField] int _itemDataBuildCount;

    public void MarketPanelStart()
    {
        ButtonPlacement();
        TextPlacement();
    }

    public void ItemSelected(InfoPanel.InfoPanelStat infoPanelStat)
    {
        if (infoPanelStat == InfoPanel.InfoPanelStat.motherbase)
            MotherBaseCancelFunc(false, true);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.barracks)
            BarracksCancelFunc(false, true);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.central)
            CentralCancelFunc(false, true);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.miner)
            MinerCancelFunc(false, true);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.repairman)
            RepairmanCancelFunc(false, true);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.hospital)
            HospitalCancelFunc(false, true);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.archer)
            ArcherCancelFunc(false, true);
    }
    public void NewItemSelected(InfoPanel.InfoPanelStat infoPanelStat)
    {
        if (infoPanelStat == InfoPanel.InfoPanelStat.motherbase)
            MotherBaseCancelFunc(true, false);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.barracks)
            BarracksCancelFunc(true, false);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.central)
            CentralCancelFunc(true, false);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.miner)
            MinerCancelFunc(true, false);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.repairman)
            RepairmanCancelFunc(true, false);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.hospital)
            HospitalCancelFunc(true, false);
        else if (infoPanelStat == InfoPanel.InfoPanelStat.archer)
            ArcherCancelFunc(true, false);
    }

    private void TextPlacement()
    {
        MotherBaseTextPlacement();
        BarracksTextPlacement();
        CentralTextPlacement();
        MinerTextPlacement();
        RepairmanTextPlacement();
        HospitalTextPlacement();
        ArcherTextPlacement();
    }
    private void ButtonPlacement()
    {
        _motherBaseBuyButton.onClick.AddListener(MotherBaseBuyFunc);
        _motherBaseCancelButton.onClick.AddListener(() => MotherBaseCancelFunc(false, false));
        _barracksBuyButton.onClick.AddListener(BarracksBuyFunc);
        _barracksCancelButton.onClick.AddListener(() => BarracksCancelFunc(false, false));
        _centralBuyButton.onClick.AddListener(CentralBuyFunc);
        _centralCancelButton.onClick.AddListener(() => CentralCancelFunc(false, false));
        _minerBuyButton.onClick.AddListener(MinerBuyFunc);
        _minerCancelButton.onClick.AddListener(() => MinerCancelFunc(false, false));
        _repairmanBuyButton.onClick.AddListener(RepairmanBuyFunc);
        _repairmanCancelButton.onClick.AddListener(() => RepairmanCancelFunc(false, false));
        _hospitalBuyButton.onClick.AddListener(HospitalBuyFunc);
        _hospitalCancelButton.onClick.AddListener(() => HospitalCancelFunc(false, false));
        _archerBuyButton.onClick.AddListener(ArcherBuyFunc);
        _archerCancelButton.onClick.AddListener(() => ArcherCancelFunc(false, false));
    }

    private void MotherBaseBuyFunc()
    {
        ItemData itemData = ItemData.Instance;
        GameManager gameManager = GameManager.Instance;
        InfoPanel infoPanel = InfoPanel.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.motherbase;

        if (itemData.fieldPrice.fields[itemDataCount] <= gameManager.money && itemData.factor.fields[itemDataCount] < itemData.maxFactor.fields[itemDataCount])
        {
            InfoPanel.InfoPanelStat tempInfoStat = infoPanel.OpenBuyInfoPanel(InfoPanel.InfoPanelStat.motherbase);

            if (tempInfoStat != InfoPanel.InfoPanelStat.free)
                NewItemSelected(tempInfoStat);

            SelectSystem.Instance.SelectBuildPlacement();
            MoneySystem.Instance.MoneyTextRevork(-(int)itemData.fieldPrice.fields[itemDataCount]);
            itemData.SetField(itemDataCount);
            MotherBaseTextPlacement();
            MotherBaseBuyVisibility();
            BuildManager.Instance.GenerateBuild((int)InfoPanel.InfoPanelStat.motherbase);
        }

    }
    private void MotherBaseCancelFunc(bool isNewPanel, bool isSelected)
    {
        ItemData itemData = ItemData.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.motherbase;

        SelectSystem.Instance.SelectFree();
        MotherBaseCancelVisibility();

        if (!isSelected)
        {
            itemData.SetBackField(itemDataCount);
            MotherBaseTextPlacement();
            MoneySystem.Instance.MoneyTextRevork((int)itemData.fieldPrice.fields[itemDataCount]);
            BuildManager.Instance.CancelBuild();
        }

        if (!isNewPanel)
            InfoPanel.Instance.CloseBuyInfoPanel();
    }
    private void BarracksBuyFunc()
    {
        ItemData itemData = ItemData.Instance;
        GameManager gameManager = GameManager.Instance;
        InfoPanel infoPanel = InfoPanel.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.barracks;

        if (FirstTapMechanic.Instance.isMotherBase && itemData.fieldPrice.fields[itemDataCount] <= gameManager.money && itemData.factor.fields[itemDataCount] < itemData.maxFactor.fields[itemDataCount])
        {
            InfoPanel.InfoPanelStat tempInfoStat = infoPanel.OpenBuyInfoPanel(InfoPanel.InfoPanelStat.barracks);

            if (tempInfoStat != InfoPanel.InfoPanelStat.free)
                NewItemSelected(tempInfoStat);

            SelectSystem.Instance.SelectBuildPlacement();
            MoneySystem.Instance.MoneyTextRevork(-(int)itemData.fieldPrice.fields[itemDataCount]);
            itemData.SetField(itemDataCount);
            BarracksTextPlacement();
            BarracksBuyVisibility();
            BuildManager.Instance.GenerateBuild((int)InfoPanel.InfoPanelStat.barracks);
        }
    }
    private void BarracksCancelFunc(bool isNewPanel, bool isSelected)
    {
        ItemData itemData = ItemData.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.barracks;

        SelectSystem.Instance.SelectFree();
        BarracksCancelVisibility();

        if (!isSelected)
        {
            itemData.SetBackField(itemDataCount);
            BarracksTextPlacement();
            MoneySystem.Instance.MoneyTextRevork((int)itemData.fieldPrice.fields[itemDataCount]);
            BuildManager.Instance.CancelBuild();
        }

        if (!isNewPanel)
            InfoPanel.Instance.CloseBuyInfoPanel();
    }
    private void CentralBuyFunc()
    {
        ItemData itemData = ItemData.Instance;
        GameManager gameManager = GameManager.Instance;
        InfoPanel infoPanel = InfoPanel.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.central;

        if (FirstTapMechanic.Instance.isMotherBase && itemData.fieldPrice.fields[itemDataCount] <= gameManager.money && itemData.factor.fields[itemDataCount] < itemData.maxFactor.fields[itemDataCount])
        {
            InfoPanel.InfoPanelStat tempInfoStat = infoPanel.OpenBuyInfoPanel(InfoPanel.InfoPanelStat.central);

            if (tempInfoStat != InfoPanel.InfoPanelStat.free)
                NewItemSelected(tempInfoStat);

            SelectSystem.Instance.SelectBuildPlacement();
            MoneySystem.Instance.MoneyTextRevork(-(int)itemData.fieldPrice.fields[itemDataCount]);
            itemData.SetField(itemDataCount);
            CentralTextPlacement();
            CentralBuyVisibility();
            BuildManager.Instance.GenerateBuild((int)InfoPanel.InfoPanelStat.central);
        }
    }
    private void CentralCancelFunc(bool isNewPanel, bool isSelected)
    {
        ItemData itemData = ItemData.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.central;

        SelectSystem.Instance.SelectFree();
        CentralCancelVisibility();

        if (!isSelected)
        {
            itemData.SetBackField(itemDataCount);
            CentralTextPlacement();
            MoneySystem.Instance.MoneyTextRevork((int)itemData.fieldPrice.fields[itemDataCount]);
            BuildManager.Instance.CancelBuild();
        }

        if (!isNewPanel)
            InfoPanel.Instance.CloseBuyInfoPanel();
    }
    private void MinerBuyFunc()
    {
        ItemData itemData = ItemData.Instance;
        GameManager gameManager = GameManager.Instance;
        InfoPanel infoPanel = InfoPanel.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.miner;

        if (FirstTapMechanic.Instance.isMotherBase && itemData.fieldPrice.fields[itemDataCount] <= gameManager.money && itemData.factor.fields[itemDataCount] < itemData.maxFactor.fields[itemDataCount])
        {
            InfoPanel.InfoPanelStat tempInfoStat = infoPanel.OpenBuyInfoPanel(InfoPanel.InfoPanelStat.miner);

            if (tempInfoStat != InfoPanel.InfoPanelStat.free)
                NewItemSelected(tempInfoStat);

            SelectSystem.Instance.SelectBuildPlacement();
            MoneySystem.Instance.MoneyTextRevork(-(int)itemData.fieldPrice.fields[itemDataCount]);
            itemData.SetField(itemDataCount);
            MinerTextPlacement();
            MinerBuyVisibility();
            BuildManager.Instance.GenerateBuild((int)InfoPanel.InfoPanelStat.miner);
        }
    }
    private void MinerCancelFunc(bool isNewPanel, bool isSelected)
    {
        ItemData itemData = ItemData.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.miner;

        SelectSystem.Instance.SelectFree();
        MinerCancelVisibility();

        if (!isSelected)
        {
            itemData.SetBackField(itemDataCount);
            MinerTextPlacement();
            MoneySystem.Instance.MoneyTextRevork((int)itemData.fieldPrice.fields[itemDataCount]);
            BuildManager.Instance.CancelBuild();
        }

        if (!isNewPanel)
            InfoPanel.Instance.CloseBuyInfoPanel();
    }
    private void RepairmanBuyFunc()
    {
        ItemData itemData = ItemData.Instance;
        GameManager gameManager = GameManager.Instance;
        InfoPanel infoPanel = InfoPanel.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.repairman;

        if (FirstTapMechanic.Instance.isMotherBase && itemData.fieldPrice.fields[itemDataCount] <= gameManager.money && itemData.factor.fields[itemDataCount] < itemData.maxFactor.fields[itemDataCount])
        {
            InfoPanel.InfoPanelStat tempInfoStat = infoPanel.OpenBuyInfoPanel(InfoPanel.InfoPanelStat.repairman);

            if (tempInfoStat != InfoPanel.InfoPanelStat.free)
                NewItemSelected(tempInfoStat);

            SelectSystem.Instance.SelectBuildPlacement();
            MoneySystem.Instance.MoneyTextRevork(-(int)itemData.fieldPrice.fields[itemDataCount]);
            itemData.SetField(itemDataCount);
            RepairmanTextPlacement();
            RepairmanBuyVisibility();
            BuildManager.Instance.GenerateBuild((int)InfoPanel.InfoPanelStat.repairman);
        }
    }
    private void RepairmanCancelFunc(bool isNewPanel, bool isSelected)
    {
        ItemData itemData = ItemData.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.repairman;

        SelectSystem.Instance.SelectFree();
        RepairmanCancelVisibility();

        if (!isSelected)
        {
            itemData.SetBackField(itemDataCount);
            RepairmanTextPlacement();
            MoneySystem.Instance.MoneyTextRevork((int)itemData.fieldPrice.fields[itemDataCount]);
            BuildManager.Instance.CancelBuild();
        }

        if (!isNewPanel)
            InfoPanel.Instance.CloseBuyInfoPanel();
    }
    private void HospitalBuyFunc()
    {
        ItemData itemData = ItemData.Instance;
        GameManager gameManager = GameManager.Instance;
        InfoPanel infoPanel = InfoPanel.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.hospital;

        if (FirstTapMechanic.Instance.isMotherBase && itemData.fieldPrice.fields[itemDataCount] <= gameManager.money && itemData.factor.fields[itemDataCount] < itemData.maxFactor.fields[itemDataCount])
        {
            InfoPanel.InfoPanelStat tempInfoStat = infoPanel.OpenBuyInfoPanel(InfoPanel.InfoPanelStat.hospital);

            if (tempInfoStat != InfoPanel.InfoPanelStat.free)
                NewItemSelected(tempInfoStat);

            SelectSystem.Instance.SelectBuildPlacement();
            MoneySystem.Instance.MoneyTextRevork(-(int)itemData.fieldPrice.fields[itemDataCount]);
            itemData.SetField(itemDataCount);
            HospitalTextPlacement();
            HospitalBuyVisibility();
            BuildManager.Instance.GenerateBuild((int)InfoPanel.InfoPanelStat.hospital);
        }
    }
    private void HospitalCancelFunc(bool isNewPanel, bool isSelected)
    {
        ItemData itemData = ItemData.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.hospital;

        SelectSystem.Instance.SelectFree();
        HospitalCancelVisibility();

        if (!isSelected)
        {
            itemData.SetBackField(itemDataCount);
            HospitalTextPlacement();
            MoneySystem.Instance.MoneyTextRevork((int)itemData.fieldPrice.fields[itemDataCount]);
            BuildManager.Instance.CancelBuild();
        }

        if (!isNewPanel)
            InfoPanel.Instance.CloseBuyInfoPanel();
    }
    private void ArcherBuyFunc()
    {
        ItemData itemData = ItemData.Instance;
        GameManager gameManager = GameManager.Instance;
        InfoPanel infoPanel = InfoPanel.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.archer;

        if (FirstTapMechanic.Instance.isMotherBase && itemData.fieldPrice.fields[itemDataCount] <= gameManager.money && itemData.factor.fields[itemDataCount] < itemData.maxFactor.fields[itemDataCount])
        {
            InfoPanel.InfoPanelStat tempInfoStat = infoPanel.OpenBuyInfoPanel(InfoPanel.InfoPanelStat.archer);

            if (tempInfoStat != InfoPanel.InfoPanelStat.free)
                NewItemSelected(tempInfoStat);

            SelectSystem.Instance.SelectBuildPlacement();
            MoneySystem.Instance.MoneyTextRevork(-(int)itemData.fieldPrice.fields[itemDataCount]);
            itemData.SetField(itemDataCount);
            ArcherTextPlacement();
            ArcherBuyVisibility();
            BuildManager.Instance.GenerateBuild((int)InfoPanel.InfoPanelStat.archer);
        }
    }
    private void ArcherCancelFunc(bool isNewPanel, bool isSelected)
    {
        ItemData itemData = ItemData.Instance;

        int itemDataCount = _itemDataBuildCount + (int)InfoPanel.InfoPanelStat.archer;

        SelectSystem.Instance.SelectFree();
        ArcherCancelVisibility();

        if (!isSelected)
        {
            itemData.SetBackField(itemDataCount);
            ArcherTextPlacement();
            MoneySystem.Instance.MoneyTextRevork((int)itemData.fieldPrice.fields[itemDataCount]);
            BuildManager.Instance.CancelBuild();
        }

        if (!isNewPanel)
            InfoPanel.Instance.CloseBuyInfoPanel();
    }

    private void MotherBaseBuyVisibility()
    {
        _motherBaseBuyButton.gameObject.SetActive(false);
        _motherBaseCancelButton.gameObject.SetActive(true);
    }
    private void MotherBaseCancelVisibility()
    {
        _motherBaseBuyButton.gameObject.SetActive(true);
        _motherBaseCancelButton.gameObject.SetActive(false);
    }
    private void BarracksBuyVisibility()
    {
        _barracksBuyButton.gameObject.SetActive(false);
        _barracksCancelButton.gameObject.SetActive(true);
    }
    private void BarracksCancelVisibility()
    {
        _barracksBuyButton.gameObject.SetActive(true);
        _barracksCancelButton.gameObject.SetActive(false);
    }
    private void CentralBuyVisibility()
    {
        _centralBuyButton.gameObject.SetActive(false);
        _centralCancelButton.gameObject.SetActive(true);
    }
    private void CentralCancelVisibility()
    {
        _centralBuyButton.gameObject.SetActive(true);
        _centralCancelButton.gameObject.SetActive(false);
    }
    private void MinerBuyVisibility()
    {
        _minerBuyButton.gameObject.SetActive(false);
        _minerCancelButton.gameObject.SetActive(true);
    }
    private void MinerCancelVisibility()
    {
        _minerBuyButton.gameObject.SetActive(true);
        _minerCancelButton.gameObject.SetActive(false);
    }
    private void RepairmanBuyVisibility()
    {
        _repairmanBuyButton.gameObject.SetActive(false);
        _repairmanCancelButton.gameObject.SetActive(true);
    }
    private void RepairmanCancelVisibility()
    {
        _repairmanBuyButton.gameObject.SetActive(true);
        _repairmanCancelButton.gameObject.SetActive(false);
    }
    private void HospitalBuyVisibility()
    {
        _hospitalBuyButton.gameObject.SetActive(false);
        _hospitalCancelButton.gameObject.SetActive(true);
    }
    private void HospitalCancelVisibility()
    {
        _hospitalBuyButton.gameObject.SetActive(true);
        _hospitalCancelButton.gameObject.SetActive(false);
    }
    private void ArcherBuyVisibility()
    {
        _archerBuyButton.gameObject.SetActive(false);
        _archerCancelButton.gameObject.SetActive(true);
    }
    private void ArcherCancelVisibility()
    {
        _archerBuyButton.gameObject.SetActive(true);
        _archerCancelButton.gameObject.SetActive(false);
    }

    private void MotherBaseTextPlacement()
    {
        ItemData itemData = ItemData.Instance;

        if (itemData.factor.fields[(int)InfoPanel.InfoPanelStat.motherbase] < itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.motherbase])
            _motherBaseCostText.text = itemData.fieldPrice.fields[(int)InfoPanel.InfoPanelStat.motherbase].ToString();
        else
            _motherBaseCostText.text = "Full";

        _motherBaseLimitText.text = itemData.field.fields[(int)InfoPanel.InfoPanelStat.motherbase] + " / " + (itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.motherbase] - 1);
    }
    private void BarracksTextPlacement()
    {
        ItemData itemData = ItemData.Instance;

        if (itemData.factor.fields[(int)InfoPanel.InfoPanelStat.barracks] < itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.barracks])
            _barracksCostText.text = itemData.fieldPrice.fields[(int)InfoPanel.InfoPanelStat.barracks].ToString();
        else
            _barracksCostText.text = "Full";

        _barracksLimitText.text = itemData.field.fields[(int)InfoPanel.InfoPanelStat.barracks] + " / " + (itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.barracks] - 1);
    }
    private void CentralTextPlacement()
    {
        ItemData itemData = ItemData.Instance;

        if (itemData.factor.fields[(int)InfoPanel.InfoPanelStat.central] < itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.central])
            _centralCostText.text = itemData.fieldPrice.fields[(int)InfoPanel.InfoPanelStat.central].ToString();
        else
            _centralCostText.text = "Full";

        _centralLimitText.text = itemData.field.fields[(int)InfoPanel.InfoPanelStat.central] + " / " + (itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.central] - 1);
    }
    private void MinerTextPlacement()
    {
        ItemData itemData = ItemData.Instance;

        if (itemData.factor.fields[(int)InfoPanel.InfoPanelStat.miner] < itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.miner])
            _minerCostText.text = itemData.fieldPrice.fields[(int)InfoPanel.InfoPanelStat.miner].ToString();
        else
            _minerCostText.text = "Full";

        _minerLimitText.text = itemData.field.fields[(int)InfoPanel.InfoPanelStat.miner] + " / " + (itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.miner] - 1);
    }
    private void RepairmanTextPlacement()
    {
        ItemData itemData = ItemData.Instance;

        if (itemData.factor.fields[(int)InfoPanel.InfoPanelStat.repairman] < itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.repairman])
            _repairmanCostText.text = itemData.fieldPrice.fields[(int)InfoPanel.InfoPanelStat.repairman].ToString();
        else
            _repairmanCostText.text = "Full";

        _repairmanLimitText.text = itemData.field.fields[(int)InfoPanel.InfoPanelStat.repairman] + " / " + (itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.repairman] - 1);
    }
    private void HospitalTextPlacement()
    {
        ItemData itemData = ItemData.Instance;

        if (itemData.factor.fields[(int)InfoPanel.InfoPanelStat.hospital] < itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.hospital])
            _hospitalCostText.text = itemData.fieldPrice.fields[(int)InfoPanel.InfoPanelStat.hospital].ToString();
        else
            _hospitalCostText.text = "Full";

        _hospitalLimitText.text = itemData.field.fields[(int)InfoPanel.InfoPanelStat.hospital] + " / " + (itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.hospital] - 1);
    }
    private void ArcherTextPlacement()
    {
        ItemData itemData = ItemData.Instance;

        if (itemData.factor.fields[(int)InfoPanel.InfoPanelStat.archer] < itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.archer])
            _archerCostText.text = itemData.fieldPrice.fields[(int)InfoPanel.InfoPanelStat.archer].ToString();
        else
            _archerCostText.text = "Full";

        _archerLimitText.text = itemData.field.fields[(int)InfoPanel.InfoPanelStat.archer] + " / " + (itemData.maxFactor.fields[(int)InfoPanel.InfoPanelStat.archer] - 1);
    }
}
