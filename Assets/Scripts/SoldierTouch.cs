using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierTouch : MonoBehaviour
{
    private void OnMouseDown()
    {
        SelectSystem.Instance.SelectSoldierMove();
        SoldierMoveSystem.Instance.SetSoldierFocus(gameObject);
    }

    private void OnEnable()
    {
        SoldierMoveSystem.Instance.SetInitialNavMeshSettingsForSoldier(gameObject);
    }
}
