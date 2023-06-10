using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildTouch : MonoBehaviour
{
    public bool isFree;
    public int verticalCount, horizontalCount;
    [SerializeField] GameObject _selectObject;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grid") )
        {
            GridID gridID = other.GetComponent<GridID>();
            GridSystem gridSystem = GridSystem.Instance;

            SelectObjectPlacement(other.gameObject);
            GridIDPlacement(gridID);
            SelectFreedom(gridID, gridSystem);
        }
    }

    public IEnumerator BuildPlacement()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.DOMove(_selectObject.transform.position, BuildManager.Instance.buildMoveTime);
        yield return new WaitForSeconds(0.2f);
        SoldierMoveSystem.Instance.NewBuildPlacement();
    }
    public void OverrideSelectObject(GameObject selectObject)
    {
        _selectObject = selectObject;
    }
    private void SelectObjectPlacement(GameObject other)
    {
        _selectObject = other;
    }
    private void SelectFreedom(GridID gridID, GridSystem gridSystem)
    {
        if (!gridSystem.mainGrid.horizontalGrids[gridID.VerticalCount].gridBool[gridID.horizontalCount])
            isFree = true;
        else
            isFree = false;
    }
    private void GridIDPlacement(GridID gridID)
    {
        verticalCount = gridID.VerticalCount;
        horizontalCount = gridID.horizontalCount;
    }
}
