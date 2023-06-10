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
    bool _isTouchSoldier;
    bool _isTouchBuild;
    bool _isDead;

    public void RivalIDStart(int level)
    {
        _level = level;
        _currentHP = _rivalData.maxHPs[_level - 1];
        SetVisibility();
        _focusBuild = SelectBuild();
        _rivalMove.SetTarget(_focusBuild);
    }
    public bool IsWayFree()
    {
        if (_isTouchBuild || _isTouchSoldier) return false;
        else return true;
    }

    public int GetCurrentHP()
    {
        return _currentHP;
    }
    public void DownHP(int HP)
    {
        _currentHP -= HP;
        CheckHealth();
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
        if (!_isTouchSoldier && collision.gameObject.CompareTag("Soldier"))
        {
            _isTouchSoldier = true;
            _focusSoldier = collision.gameObject;
            _rivalMove.NavmeshStoped();
            StartCoroutine(FightSoldier());
        }
        else if (!_isTouchBuild && !_isTouchSoldier && collision.gameObject.CompareTag("Build"))
        {
            _isTouchBuild = true;
            _focusBuild = collision.gameObject;
            _rivalMove.NavmeshStoped();
            StartCoroutine(FightBuild());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isDead && collision.gameObject.CompareTag("Soldier"))
        {
            _isTouchSoldier = false;
            _focusSoldier = null;
            _rivalMove.NavmeshResume();
        }
        if (!_isDead && collision.gameObject.CompareTag("Build"))
        {
            _isTouchBuild = false;
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

        while (soldierID.GetCurrentHP() > 0 && GetCurrentHP() > 0)
        {
            HitSoldier(soldierID);

            yield return new WaitForSeconds(_rivalData.hitCountDowns[_level - 1]);

            SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.idle);
        }

        if (GetCurrentHP() > 0)
            _rivalMove.NavmeshResume();

        _isTouchSoldier = false;
    }

    private IEnumerator FightBuild()
    {
        InGameSelectedSystem inGameSelectedSystem = _focusBuild.GetComponent<InGameSelectedSystem>();

        while (!_isTouchSoldier && inGameSelectedSystem.GetHealth() > 0 && GetCurrentHP() > 0)
        {
            HitBuild(inGameSelectedSystem);

            yield return new WaitForSeconds(_rivalData.hitCountDowns[_level - 1]);

            SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.idle);
        }

        if (GetCurrentHP() > 0)
            _rivalMove.NavmeshResume();

        _isTouchBuild = false;
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
    private void CheckHealth()
    {
        if (_currentHP <= 0 && !_isDead)
        {
            _isDead = true;
            ParticalManager.Instance.CallMoneyPartical(gameObject);
            RivalSpawnSystem.Instance.DownRival(gameObject);
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
    private GameObject SelectBuild()
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
