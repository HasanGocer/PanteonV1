using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuildTouch : MonoBehaviour
{
    [SerializeField] SpriteRenderer _background;
    [SerializeField] List<BuildTouch> _buildTouches = new List<BuildTouch>();
    public BuildTouch mainBuildTouch;
    public bool isReady;

    public bool CheckPlacementValidity()
    {
        BuildTouch buildTouch;
        isReady = true;

        for (int i = 0; i < _buildTouches.Count; i++)
        {
            buildTouch = _buildTouches[i];

            if (!buildTouch.isFree) isReady = false;
        }
        return isReady;
    }
    public void DrawGreen()
    {
        BuildManager.Instance.SetMaterialGreen(_background);
    }
    public void SetBackgroundColorByPlacementValidity(bool tempCheck)
    {
        BuildManager buildManager = BuildManager.Instance;

        if (tempCheck)
            buildManager.SetMaterialGreen(_background);
        else
            buildManager.SetMaterialRed(_background);
    }
    public void PlaceBuildingOnGridID()
    {
        SaveEmptyGridSpaces();
        SaveBuildingToGrid();
        GameManager.Instance.GridPlacementWrite(GridSystem.Instance.mainGrid);
    }
    public BuildTouch GetBuildTouch()
    {
        return mainBuildTouch;
    }
    public bool CheckGridIDOccupancy()
    {
        bool isChecked = true;
        for (int i1 = 0; i1 < _buildTouches.Count; i1++)
            for (int i2 = 0; i2 < _buildTouches.Count; i2++)
                if (_buildTouches[i1] == _buildTouches[i2] && i1 != i2) isChecked = false;

        return isChecked;
    }

    private void SaveEmptyGridSpaces()
    {
        GridSystem gridSystem = GridSystem.Instance;

        foreach (BuildTouch buildTouch in _buildTouches)
        {
            gridSystem.mainGrid.horizontalGrids[buildTouch.verticalCount].gridBool[buildTouch.horizontalCount] = true;
            gridSystem.mainGrid.horizontalGrids[buildTouch.verticalCount].gridGameObject[buildTouch.horizontalCount] = gameObject;
        }
    }
    private void SaveBuildingToGrid()
    {
        GridSystem gridSystem = GridSystem.Instance;

        gridSystem.mainGrid.buildTypes.Add(GetComponent<InGameSelectedSystem>().mainBuildStat);
        gridSystem.mainGrid.builds.Add(gameObject);
        gridSystem.mainGrid.buildLevel.Add(1);


        GridSystem.BuildID buildID = new GridSystem.BuildID();

        buildID.buildIntHorizontal = mainBuildTouch.horizontalCount;
        buildID.buildIntVertical = mainBuildTouch.verticalCount;

        GridSystem.Instance.mainGrid.buildID.Add(buildID);
    }
}
