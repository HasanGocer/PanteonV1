using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoSingleton<BuildManager>
{
    [Header("Build_Field")]
    [Space(10)]

    [SerializeField] int _OPBuildCount;
    [SerializeField] Material _redMaterial, _greenMaterial;
    [SerializeField] BuildData _buildData;

    [Header("Build_System_Field")]
    [Space(10)]

    [SerializeField] GameObject _buildParent;
    [SerializeField] GameObject _buildSpawnPos;

    MainBuildTouch _mainBuildTouch;
    Vector3 _mousePos;

    public float buildMoveTime;



    public void SetMainBuildTouchToNull()
    {
        _mainBuildTouch = null;
    }
    public void BuildPlacement()
    {
        if (_mainBuildTouch.isReady && _mainBuildTouch.CheckGrid())
        {
            InGameSelectedSystem inGameSelectedSystem = _mainBuildTouch.GetComponent<InGameSelectedSystem>();

            SelectSystem.Instance.SelectFree();
            DataPlacement(true, 1, inGameSelectedSystem, inGameSelectedSystem.mainBuildStat);
            SoundSystem.Instance.CallBuildPlacement();
            ParticalManager.Instance.CallBuildPartical(_mainBuildTouch.gameObject);
            StartCoroutine(_mainBuildTouch.mainBuildTouch.BuildPlacement());
            MarketPanel.Instance.ItemSelected(InfoPanel.Instance.GetBuyInfoPanelStat());
            inGameSelectedSystem.BuildPlacement();
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
                DeleteBuildFromGridSystem(gridSystem, i);
                SetFactor(build);
                GameManager.Instance.GridPlacementWrite(GridSystem.Instance.mainGrid);
                break;
            }
    }

    public void DataPlacement(bool isNew, int level, InGameSelectedSystem inGameSelectedSystem, InfoPanel.InfoPanelStat infoPanelStat)
    {
        if (isNew)
        {
            GridSystem.Instance.mainGrid.buildHP.Add(_buildData.buildMainDatas[(int)infoPanelStat].HPs[0]);
            inGameSelectedSystem.SetHealth(_buildData.buildMainDatas[(int)infoPanelStat].HPs[0]);
        }
        else
            inGameSelectedSystem.SetHealth(GridSystem.Instance.mainGrid.buildHP[GridSystem.Instance.mainGrid.builds.Count - 1]); ;

        inGameSelectedSystem.SetLevel(level);
        inGameSelectedSystem.startFunc();
    }

    public void SetMainBuildTouch(MainBuildTouch mainBuildTouch)
    {
        _mainBuildTouch = mainBuildTouch;
    }

    public MainBuildTouch GetMainBuildTouch()
    {
        return _mainBuildTouch;
    }

    public GameObject GenerateBuild(int buildCount)
    {
        GameObject build = ObjectPool.Instance.GetPooledObjectAdd(buildCount + _OPBuildCount, _buildSpawnPos.transform.position, Vector3.zero, _buildParent.transform);
        _mainBuildTouch = build.GetComponent<MainBuildTouch>();
        return build;
    }
    public void CancelBuild()
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
    private void DeleteBuildFromGridSystem(GridSystem gridSystem, int i)
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
