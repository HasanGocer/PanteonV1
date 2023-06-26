using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalSpawnSystem : MonoSingleton<RivalSpawnSystem>
{
    [SerializeField] int _OPRivalCount;
    public List<GameObject> rivals = new List<GameObject>();
    [SerializeField] GameObject _rivalParent;

    public void RivalWavePlacement()
    {
        GameManager gameManager = GameManager.Instance;

        for (int i = 0; i < gameManager.level / 1; i++)
            RivalPlacement(1);
        for (int i = 0; i < gameManager.level / 10; i++)
            RivalPlacement(2);
        for (int i = 0; i < gameManager.level / 20; i++)
            RivalPlacement(3);

        FinishSystem.Instance.SetRivalCount(gameManager.level / 1 + gameManager.level / 10 + gameManager.level / 20);
    }
    public void DownRival(GameObject rival)
    {
        rivals.Remove(rival);
    }

    private void RivalPlacement(int rivalLevelCount)
    {
        GameObject rival = ObjectPool.Instance.GetPooledObjectAdd(_OPRivalCount, RivalPlacement(), Vector3.zero, _rivalParent.transform);

        rivals.Add(rival);
        SoldierMoveSystem.Instance.SetSoldier(rival);
        rival.GetComponent<RivalID>().RivalIDStart(rivalLevelCount);
    }
    private Vector2 RivalPlacement()
    {
        GridSystem gridSystem = GridSystem.Instance;

        Vector2 rivalPos = gridSystem.GetSpawnPos();

        int tempRandom = Random.Range(0, 3);

        if (tempRandom == 0)
        {
            rivalPos += new Vector2(Random.Range(0, gridSystem.GetGridHorizontalCount()), gridSystem.GetGridVerticalCount());
        }
        else if (tempRandom == 1)
        {
            rivalPos += new Vector2(Random.Range(0, gridSystem.GetGridHorizontalCount()), 0);
        }
        else if (tempRandom == 2)
        {
            rivalPos += new Vector2(0, Random.Range(0, gridSystem.GetGridVerticalCount()));
        }
        else
        {
            rivalPos += new Vector2(gridSystem.GetGridHorizontalCount(), Random.Range(0, gridSystem.GetGridVerticalCount()));
        }

        return rivalPos;
    }
}
