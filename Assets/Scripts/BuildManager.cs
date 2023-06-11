using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoSingleton<BuildManager>
{
    [Header("Build_Field")]
    [Space(10)]

    [SerializeField] int _OPBuildCount;
    [SerializeField] Material _redMaterial, _greenMaterial;

    [Header("Build_System_Field")]
    [Space(10)]

    [SerializeField] GameObject _buildParent;
    [SerializeField] GameObject _buildSpawnPos;

    MainBuildTouch _mainBuildTouch;
    Vector3 _mousePos;

    public float buildMoveTime;



    public void ClearMainBuildTouch()
    {
        _mainBuildTouch = null;
    }
    public void BuildPlacement()
    {
        if (_mainBuildTouch.isReady && _mainBuildTouch.CheckGrid())
        {
            SelectSystem.Instance.SelectFree();
            DataPlacement(true, 1, _mainBuildTouch.GetComponent<InGameSelectedSystem>().mainBuildStat);
            SoundSystem.Instance.CallBuildPlacement();
            ParticalManager.Instance.CallBuildPartical(_mainBuildTouch.gameObject);
            StartCoroutine(_mainBuildTouch.mainBuildTouch.BuildPlacement());
            MarketPanel.Instance.ItemSelected(InfoPanel.Instance.GetBuyInfoPanelStat());
            _mainBuildTouch.GetComponent<InGameSelectedSystem>().BuildPlacement();
            _mainBuildTouch.SaveGridID();
            FirstTapMechanic.Instance.FTStart();
            _mainBuildTouch = null;
        }
    }
    public void DeleteBuild(GameObject build)
    {
        GridSystem gridSystem = GridSystem.Instance;

        for (int i = 0; i < gridSystem.mainGrid.builds.Count; i++)
            if (gridSystem.mainGrid.builds[i] == build)
            {
                DeleteBuild(gridSystem, i);
                SetFactor(build);
                GameManager.Instance.GridPlacementWrite(GridSystem.Instance.mainGrid);
                break;
            }
    }

    public void DataPlacement(bool isNew, int level, InfoPanel.InfoPanelStat ýnfoPanelStat)
    {
        if (ýnfoPanelStat == InfoPanel.InfoPanelStat.motherbase) _mainBuildTouch.gameObject.GetComponent<MotherBaseID>().StartDataPlacement(isNew);
        else if (ýnfoPanelStat == InfoPanel.InfoPanelStat.repairman) _mainBuildTouch.gameObject.GetComponent<RepairmanID>().StartDataPlacement(isNew);
        else if (ýnfoPanelStat == InfoPanel.InfoPanelStat.miner) _mainBuildTouch.gameObject.GetComponent<MinerID>().StartDataPlacement(isNew, level);
        else if (ýnfoPanelStat == InfoPanel.InfoPanelStat.hospital) _mainBuildTouch.gameObject.GetComponent<HospitalID>().StartDataPlacement(isNew);
        else if (ýnfoPanelStat == InfoPanel.InfoPanelStat.central) _mainBuildTouch.gameObject.GetComponent<CentralID>().StartDataPlacement(isNew, level);
        else if (ýnfoPanelStat == InfoPanel.InfoPanelStat.barracks) _mainBuildTouch.gameObject.GetComponent<BarracksID>().StartDataPlacement(isNew, level);
        else if (ýnfoPanelStat == InfoPanel.InfoPanelStat.archer) _mainBuildTouch.gameObject.GetComponent<ArcherID>().StartDataPlacement(isNew, level);
    }

    public void AddMainBuildTouch(MainBuildTouch mainBuildTouch)
    {
        _mainBuildTouch = mainBuildTouch;
    }

    public MainBuildTouch GetMainBuildTouch()
    {
        return _mainBuildTouch;
    }

    public GameObject GetBuild(int buildCount)
    {
        GameObject build = ObjectPool.Instance.GetPooledObjectAdd(buildCount + _OPBuildCount, _buildSpawnPos.transform.position, Vector3.zero, _buildParent.transform);
        _mainBuildTouch = build.GetComponent<MainBuildTouch>();
        return build;
    }
    public void GetBackObject()
    {
        _mainBuildTouch.gameObject.SetActive(false);
        _mainBuildTouch = null;
    }

    public void SetMaterialRed(SpriteRenderer background)
    {
        background.color = _redMaterial.color;
    }
    public void SetMaterialGreen(SpriteRenderer background)
    {
        background.color = _greenMaterial.color;
    }

    public void Update()
    {
        if (_mainBuildTouch != null && !InfoPanel.Instance.BuyInfoPanelStatIsFull())
        {
            _mainBuildTouch.DrawBackground(_mainBuildTouch.CheckBuilds());
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;
        _mainBuildTouch.transform.position = _mousePos;
    }
    private void DeleteBuild(GridSystem gridSystem, int i)
    {
        gridSystem.mainGrid.buildID.RemoveAt(i);
        gridSystem.mainGrid.buildLevel.RemoveAt(i);
        gridSystem.mainGrid.builds.RemoveAt(i);
        gridSystem.mainGrid.buildTypes.RemoveAt(i);
    }
    private void SetFactor(GameObject build)
    {
        ItemData.Instance.SetBackField((int)build.GetComponent<InGameSelectedSystem>().mainBuildStat);
    }
}
