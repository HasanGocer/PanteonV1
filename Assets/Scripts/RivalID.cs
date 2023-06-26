using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RivalID : MonoBehaviour
{
    [SerializeField] RivalData _rivalData;
    [SerializeField] RivalMove _rivalMove;
    [SerializeField] List<GameObject> _rivals = new List<GameObject>();
    [SerializeField] List<RivalAnim> _rivalAnims = new List<RivalAnim>();
    [SerializeField] int _level;
    [SerializeField] Image _barImage;

    int _currentHP;
    SoldierMoveSystem.SoldierAnimType _rivalAnimType;

    GameObject _focusBuild;
    GameObject _focusSoldier;

    [SerializeField] RivalSpawnSystem.RivalStat _rivalStat;

    public void RivalIDStart(int level)
    {
        _level = level;
        _currentHP = _rivalData.maxHPs[_level - 1];
        SetVisibility();
        _focusBuild = FindNearestBuilding();
        _rivalMove.SetTarget(_focusBuild);
    }

    public int GetRivalHealth()
    {
        return _currentHP;
    }
    public void DecreaseRivalHealth(int HP)
    {
        _currentHP -= HP;
        ParticalManager.Instance.CallBloodPartical(gameObject);
        CheckIfAlive();
    }

    public void SetSoldierAnim(SoldierMoveSystem.SoldierAnimType rivalAnimType)
    {
        if (rivalAnimType == SoldierMoveSystem.SoldierAnimType.run)
            _rivalAnims[_level - 1].PlayRunAnim();
        else if (rivalAnimType == SoldierMoveSystem.SoldierAnimType.idle)
            _rivalAnims[_level - 1].PlayIdleAnim();
        else if (rivalAnimType == SoldierMoveSystem.SoldierAnimType.attack)
            _rivalAnims[_level - 1].PlayAttackAnim();

        _rivalAnimType = rivalAnimType;
    }


    private void Update()
    {
        if (CheckBar((float)_currentHP / (float)_rivalData.maxHPs[_level - 1]))
            BarUpdate((float)_currentHP / (float)_rivalData.maxHPs[_level - 1]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_rivalStat != RivalSpawnSystem.RivalStat.focusSoldier && collision.gameObject.CompareTag("Soldier"))
        {
            _rivalStat = RivalSpawnSystem.RivalStat.focusSoldier;
            _focusSoldier = collision.gameObject;
            _rivalMove.NavmeshStoped();
            StartCoroutine(FightSoldier());
        }
        else if (_rivalStat != RivalSpawnSystem.RivalStat.focusBuild && _rivalStat != RivalSpawnSystem.RivalStat.focusSoldier && collision.gameObject.CompareTag("Build"))
        {
            _rivalStat = RivalSpawnSystem.RivalStat.focusBuild;
            _focusBuild = collision.gameObject;
            _rivalMove.NavmeshStoped();
            StartCoroutine(FightBuild());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_rivalStat != RivalSpawnSystem.RivalStat.dead && collision.gameObject.CompareTag("Soldier"))
        {
            _rivalStat = RivalSpawnSystem.RivalStat.free;
            _focusSoldier = null;
            _rivalMove.NavmeshResume();
        }
        if (_rivalStat != RivalSpawnSystem.RivalStat.dead && collision.gameObject.CompareTag("Build"))
        {
            _rivalStat = RivalSpawnSystem.RivalStat.free;
            _focusBuild = null;
            _rivalMove.NavmeshResume();
        }
    }

    private void SetVisibility()
    {
        _rivals[_level - 1].SetActive(true);
    }
    private IEnumerator FightSoldier()
    {
        SoldierID soldierID = _focusSoldier.GetComponent<SoldierID>();

        while (soldierID.GetCurrentHP() > 0 && GetRivalHealth() > 0)
        {
            HitSoldier(soldierID);

            yield return new WaitForSeconds(_rivalData.hitCountDowns[_level - 1]);

            SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.idle);
        }

        if (GetRivalHealth() > 0)
            _rivalMove.NavmeshResume();

        _rivalStat = RivalSpawnSystem.RivalStat.focusSoldier;
    }

    private IEnumerator FightBuild()
    {
        InGameSelectedSystem inGameSelectedSystem = _focusBuild.GetComponent<InGameSelectedSystem>();

        while (_rivalStat != RivalSpawnSystem.RivalStat.focusSoldier && inGameSelectedSystem.GetHealth() > 0 && GetRivalHealth() > 0)
        {
            HitBuild(inGameSelectedSystem);

            yield return new WaitForSeconds(_rivalData.hitCountDowns[_level - 1]);

            SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.idle);
        }

        if (GetRivalHealth() > 0)
            _rivalMove.NavmeshResume();

        _rivalStat = RivalSpawnSystem.RivalStat.focusBuild;
    }

    private bool CheckBar(float rateHP)
    {
        if (rateHP == _barImage.fillAmount) return false;
        else return true;
    }
    private void BarUpdate(float rateHP)
    {
        _barImage.fillAmount = Mathf.Lerp(_barImage.fillAmount, rateHP, Time.deltaTime);
    }

    private void HitSoldier(SoldierID soldierID)
    {
        SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.attack);
        soldierID.DownHP(_rivalData.damages[_level - 1]);
    }
    private void HitBuild(InGameSelectedSystem inGameSelectedSystem)
    {
        SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.attack);
        SoundSystem.Instance.CallBuildHit();
        inGameSelectedSystem.SetHealth(inGameSelectedSystem.GetHealth() - _rivalData.damages[_level - 1]);
    }
    private void CheckIfAlive()
    {
        if (_currentHP <= 0 && _rivalStat != RivalSpawnSystem.RivalStat.dead)
        {
            _rivalStat = RivalSpawnSystem.RivalStat.dead;
            ParticalManager.Instance.CallMoneyPartical(gameObject);
            RivalSpawnSystem.Instance.ClearDeadRivals(gameObject);
            SetBar();
            TextSystem.Instance.PlusTime(gameObject);
            SoundSystem.Instance.CallBuildPlacement();
            MoneySystem.Instance.MoneyTextRevork(InfoPanel.Instance.soldierCost);
            _rivals[_level - 1].SetActive(false);
            FinishSystem.Instance.FinishCheck();
            _rivalAnimType = SoldierMoveSystem.SoldierAnimType.idle;
            gameObject.SetActive(false);
        }
    }
    private void SetBar()
    {
        _barImage.fillAmount = 1;
    }
    private GameObject FindNearestBuilding()
    {
        GridSystem gridSystem = GridSystem.Instance;

        GameObject tempBuild = gridSystem.mainGrid.builds[0];

        for (int i = 1; i < gridSystem.mainGrid.builds.Count; i++)
            if (CheckDistance(tempBuild, gridSystem.mainGrid.builds[i]))
                tempBuild = gridSystem.mainGrid.builds[i];

        return tempBuild;
    }

    private bool CheckDistance(GameObject tempBuild, GameObject newBuild)
    {
        if (Vector3.Distance(tempBuild.transform.position, transform.position) > Vector3.Distance(newBuild.transform.position, transform.position)) return true;
        else return false;
    }
}
