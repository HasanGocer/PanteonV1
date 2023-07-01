using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketPanel : MonoSingleton<MarketPanel>
{
    [Header("Buttons")]
    [Space(10)]

    [SerializeField] List<Button> _buyButtons = new List<Button>();
    [SerializeField] List<Button> _cancelButtons = new List<Button>();

    [Header("Cost_Text")]
    [Space(10)]

    [SerializeField] List<TMP_Text> _costTexts = new List<TMP_Text>();

    [Header("Limit_Text")]
    [Space(10)]

    [SerializeField] List<TMP_Text> _limitTexts = new List<TMP_Text>();

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
        BuildCancelFunc(infoPanelStat, false, true);
    }
    public void NewItemSelected(InfoPanel.InfoPanelStat infoPanelStat)
    {
        BuildCancelFunc(infoPanelStat, true, false);
    }

    private void TextPlacement()
    {
        for (InfoPanel.InfoPanelStat i = InfoPanel.InfoPanelStat.motherbase; i < InfoPanel.InfoPanelStat.archer; i++)
            BuildTextPlacement(i);
    }
    private void ButtonPlacement()
    {
        _buyButtons[0].onClick.AddListener(() => BuildBuyFunc(InfoPanel.InfoPanelStat.motherbase));
        _cancelButtons[0].onClick.AddListener(() => BuildCancelFunc(InfoPanel.InfoPanelStat.motherbase, false, false));
        _buyButtons[1].onClick.AddListener(() => BuildBuyFunc(InfoPanel.InfoPanelStat.barracks));
        _cancelButtons[1].onClick.AddListener(() => BuildCancelFunc(InfoPanel.InfoPanelStat.barracks, false, false));
        _buyButtons[2].onClick.AddListener(() => BuildBuyFunc(InfoPanel.InfoPanelStat.central));
        _cancelButtons[2].onClick.AddListener(() => BuildCancelFunc(InfoPanel.InfoPanelStat.central, false, false));
        _buyButtons[3].onClick.AddListener(() => BuildBuyFunc(InfoPanel.InfoPanelStat.miner));
        _cancelButtons[3].onClick.AddListener(() => BuildCancelFunc(InfoPanel.InfoPanelStat.miner, false, false));
        _buyButtons[4].onClick.AddListener(() => BuildBuyFunc(InfoPanel.InfoPanelStat.repairman));
        _cancelButtons[4].onClick.AddListener(() => BuildCancelFunc(InfoPanel.InfoPanelStat.repairman, false, false));
        _buyButtons[5].onClick.AddListener(() => BuildBuyFunc(InfoPanel.InfoPanelStat.hospital));
        _cancelButtons[5].onClick.AddListener(() => BuildCancelFunc(InfoPanel.InfoPanelStat.hospital, false, false));
        _buyButtons[6].onClick.AddListener(() => BuildBuyFunc(InfoPanel.InfoPanelStat.archer));
        _cancelButtons[6].onClick.AddListener(() => BuildCancelFunc(InfoPanel.InfoPanelStat.archer, false, false));
    }

    private void BuildBuyFunc(InfoPanel.InfoPanelStat buildStat)
    {
        ItemData itemData = ItemData.Instance;
        GameManager gameManager = GameManager.Instance;
        InfoPanel infoPanel = InfoPanel.Instance;

        int itemDataCount = _itemDataBuildCount + (int)buildStat;

        if (FirstTapMechanic.Instance.isMotherBase || buildStat == InfoPanel.InfoPanelStat.motherbase)
            if (itemData.fieldPrice.fields[itemDataCount] <= gameManager.money && itemData.factor.fields[itemDataCount] < itemData.maxFactor.fields[itemDataCount])
            {
                InfoPanel.InfoPanelStat tempInfoStat = infoPanel.OpenBuyInfoPanel(buildStat);

                if (tempInfoStat != InfoPanel.InfoPanelStat.free)
                    NewItemSelected(buildStat);

                SelectSystem.Instance.SelectBuildPlacement();
                MoneySystem.Instance.MoneyTextRevork(-(int)itemData.fieldPrice.fields[itemDataCount]);
                itemData.SetField(itemDataCount);
                BuildTextPlacement(buildStat);
                BuildBuyVisibility(buildStat, true);
                BuildManager.Instance.GenerateBuild((int)buildStat);
            }
    }

    private void BuildCancelFunc(InfoPanel.InfoPanelStat buildStat, bool isNewPanel, bool isSelected)
    {
        ItemData itemData = ItemData.Instance;

        int itemDataCount = _itemDataBuildCount + (int)buildStat;

        SelectSystem.Instance.SelectFree();
        BuildBuyVisibility(buildStat, false);

        if (!isSelected)
        {
            itemData.SetBackField(itemDataCount);
            BuildTextPlacement(buildStat);
            MoneySystem.Instance.MoneyTextRevork((int)itemData.fieldPrice.fields[itemDataCount]);
            BuildManager.Instance.CancelBuild();
        }

        if (!isNewPanel)
            InfoPanel.Instance.CloseBuyInfoPanel();
    }


    private void BuildBuyVisibility(InfoPanel.InfoPanelStat buildType, bool isOpenBuyPanel)
    {
        if (isOpenBuyPanel)
        {
            _buyButtons[(int)buildType].gameObject.SetActive(false);
            _cancelButtons[(int)buildType].gameObject.SetActive(true);
        }
        else
        {
            _buyButtons[(int)buildType].gameObject.SetActive(true);
            _cancelButtons[(int)buildType].gameObject.SetActive(false);
        }
    }

    private void BuildTextPlacement(InfoPanel.InfoPanelStat buildStat)
    {
        ItemData itemData = ItemData.Instance;

        if (itemData.factor.fields[(int)buildStat] < itemData.maxFactor.fields[(int)buildStat])
            _costTexts[(int)buildStat].text = itemData.fieldPrice.fields[(int)buildStat].ToString();
        else
            _costTexts[(int)buildStat].text = "Full";

        _limitTexts[(int)buildStat].text = itemData.field.fields[(int)buildStat] + " / " + (itemData.maxFactor.fields[(int)buildStat] - 1);
    }
}
