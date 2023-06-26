using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoSingleton<GridSystem>
{
    [System.Serializable]
    public class HorizontalGrid
    {
        public List<GameObject> gridGameObject = new List<GameObject>();
        public List<bool> gridBool = new List<bool>();
    }
    [System.Serializable]
    public class BuildID
    {
        public int buildIntVertical;
        public int buildIntHorizontal;
    }
    [System.Serializable]
    public class VerticalGrid
    {
        public List<HorizontalGrid> horizontalGrids = new List<HorizontalGrid>();
        public List<BuildID> buildID = new List<BuildID>();
        public List<GameObject> builds = new List<GameObject>();
        public List<int> buildLevel = new List<int>();
        public List<int> buildHP = new List<int>();
        public List<InfoPanel.InfoPanelStat> buildTypes = new List<InfoPanel.InfoPanelStat>();
        public List<int> soldierCount = new List<int>();
        public bool isFinish;
    }

    public VerticalGrid mainGrid = new VerticalGrid();

    [SerializeField] GameObject _gridParentGameObject;

    [SerializeField] Color _baseColor, _offsetColor;
    [SerializeField] int _verticalGridCount, _horizontalGridCount;
    [SerializeField] SpriteRenderer _templateRenderer;
    [SerializeField] GameObject _gridSpawnPos;

    public void NewGameGridPlacement()
    {
        for (int i1 = 0; i1 < _verticalGridCount; i1++)
        {
            NewGridPlacement();

            for (int i2 = 0; i2 < _horizontalGridCount; i2++)
            {
                var spawnRenderer = Instantiate(_templateRenderer, new Vector2(_gridSpawnPos.transform.position.x + i2, _gridSpawnPos.transform.position.y + i1), Quaternion.identity);

                GridParentPlacement(spawnRenderer.gameObject);
                NewGridHorizontalPlacement(i1, spawnRenderer.gameObject);
                GridIDPlacement(spawnRenderer.gameObject, i1, i2);
                BaseColorPlacement(spawnRenderer, BoolOffsetPlacement(i2, i1));
            }
        }
    }
    public void ResumeGameGridPlacement()
    {
        GameObject build;

        for (int i1 = 0; i1 < _verticalGridCount; i1++)
        {
            ResumeGridPlacement(i1);

            for (int i2 = 0; i2 < _horizontalGridCount; i2++)
            {
                var spawnRenderer = Instantiate(_templateRenderer, new Vector2(_gridSpawnPos.transform.position.x + i2, _gridSpawnPos.transform.position.y + i1), Quaternion.identity);

                GridParentPlacement(spawnRenderer.gameObject);
                ResumeGridHorizontalPlacement(i1, spawnRenderer.gameObject);
                GridIDPlacement(spawnRenderer.gameObject, i1, i2);
                BaseColorPlacement(spawnRenderer, BoolOffsetPlacement(i2, i1));
            }
        }

        GridBuildClear();

        for (int i = 0; i < mainGrid.buildTypes.Count; i++)
        {
            build = BuildManager.Instance.GenerateBuild((int)mainGrid.buildTypes[i]);

            BuildGridAdded(build);
            BuildPlacement(mainGrid.buildLevel[i], ref build, mainGrid.buildID[i]);
        }

        for (int i1 = 0; i1 < 3; i1++)
            for (int i2 = 0; i2 < mainGrid.soldierCount[i1]; i2++)
            {
                GameObject soldier = SoliderSpawnSystem.Instance.GetSolider(i1 + 1, _gridSpawnPos.transform.position);
                soldier.GetComponent<SoldierID>().StatPlacement(i1 + 1);
            }
    }

    public void SoldierAdded(int level)
    {
        mainGrid.soldierCount[level - 1]++;

        GameManager.Instance.GridPlacementWrite(mainGrid);
    }

    public int GetHorizontalGridCount()
    {
        return _horizontalGridCount;
    }
    public int GetVerticalGridCount()
    {
        return _verticalGridCount;
    }
    public Vector2 GetSpawnPos()
    {
        return _gridSpawnPos.transform.position;
    }

    private void GridIDPlacement(GameObject grid, int verticalCount, int horizontalCount)
    {
        GridID gridID = grid.GetComponent<GridID>();

        gridID.VerticalCount = verticalCount;
        gridID.horizontalCount = horizontalCount;
    }
    private void NewGridPlacement()
    {
        HorizontalGrid tempHorizontal = new HorizontalGrid();
        mainGrid.horizontalGrids.Add(tempHorizontal);
    }
    private void NewGridHorizontalPlacement(int verticalCount, GameObject horizontalObject)
    {
        mainGrid.horizontalGrids[verticalCount].gridGameObject.Add(horizontalObject);
        mainGrid.horizontalGrids[verticalCount].gridBool.Add(false);
    }
    private void ResumeGridPlacement(int verticalCount)
    {
        mainGrid.horizontalGrids[verticalCount].gridGameObject.Clear();
    }
    private void ResumeGridHorizontalPlacement(int verticalCount, GameObject horizontalObject)
    {
        mainGrid.horizontalGrids[verticalCount].gridGameObject.Add(horizontalObject);
    }
    private void BuildPlacement(int level, ref GameObject build, BuildID buildID)
    {
        MainBuildTouch mainBuildTouch = build.GetComponent<MainBuildTouch>();
        InGameSelectedSystem inGameSelectedSystem = build.GetComponent<InGameSelectedSystem>();
        BuildTouch buildTouch = mainBuildTouch.GetBuildTouch();

        BuildManager.Instance.SetMainBuildTouch(mainBuildTouch);
        buildTouch.OverrideSelectObject(mainGrid.horizontalGrids[buildID.buildIntVertical].gridGameObject[buildID.buildIntHorizontal]);
        StartCoroutine(buildTouch.BuildPlacement());
        inGameSelectedSystem.BuildPlacement();
        BuildManager.Instance.DataPlacement(false, level, inGameSelectedSystem, inGameSelectedSystem.mainBuildStat);
        mainBuildTouch.DrawGreen();
        BuildManager.Instance.SetMainBuildTouchToNull();
    }

    private void GridParentPlacement(GameObject grid)
    {
        grid.transform.SetParent(_gridParentGameObject.transform);
    }
    private bool BoolOffsetPlacement(int x, int y)
    {
        return (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
    }
    private void GridBuildClear()
    {
        mainGrid.builds.Clear();
    }
    private void BuildGridAdded(GameObject build)
    {
        mainGrid.builds.Add(build);
    }
    private void BaseColorPlacement(SpriteRenderer renderer, bool isOffset)
    {
        renderer.color = isOffset ? _offsetColor : _baseColor;
    }
}
