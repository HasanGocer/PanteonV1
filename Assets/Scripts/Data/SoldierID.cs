using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SoldierID : MonoBehaviour
{
    [SerializeField] SoldierData _SoldierData;

    [SerializeField] List<GameObject> _soldiers = new List<GameObject>();
    [SerializeField] List<SoldierAnim> _soldierAnims = new List<SoldierAnim>();
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Image _barImage;
    [SerializeField] int _level;

    int _currentHP;
    bool _isFollowRival;
    bool _isFighting;
    bool _isDead;
    GameObject _focusRival;
    SoldierMoveSystem.SoldierAnimType _soldierAnimType;

    public int GetCurrentHP()
    {
        return _currentHP;
    }

    public void StatPlacement(int level)
    {
        LevelPlacement(level);
        SoldierVisibility(level);
        HPPlacement(level);
        GridSystem.Instance.saveSoldierRecord(level);
    }

    public void SetSoldierAnim(SoldierMoveSystem.SoldierAnimType soldierAnimType)
    {
        _soldierAnimType = soldierAnimType;

        if (soldierAnimType == SoldierMoveSystem.SoldierAnimType.run)
            _soldierAnims[_level - 1].PlayRunAnim();
        else if (soldierAnimType == SoldierMoveSystem.SoldierAnimType.idle)
            _soldierAnims[_level - 1].PlayIdleAnim();
        else if (soldierAnimType == SoldierMoveSystem.SoldierAnimType.attack)
            _soldierAnims[_level - 1].PlayAttackAnim();
        else if (soldierAnimType == SoldierMoveSystem.SoldierAnimType.dead)
            _soldierAnims[_level - 1].PlayDeadAnim();
    }

    public void FollowRival(GameObject rival)
    {
        _isFollowRival = true;
        _focusRival = rival;
    }

    public void DownHP(int HP)
    {
        _currentHP -= HP;
        CheckHealth();
    }

    public void SoldierHeal()
    {
        _currentHP = _SoldierData.maxHPs[_level - 1];
        ParticalManager.Instance.CallHealthPartical(gameObject);
    }

    private void Update()
    {
        if (_navMeshAgent.remainingDistance == 0 && _soldierAnimType == SoldierMoveSystem.SoldierAnimType.run) SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.idle);
        else if (_isFollowRival) FollowRival();

        if (CheckBar((float)_currentHP / (float)_SoldierData.maxHPs[_level - 1]))
            BarUpdate((float)_currentHP / (float)_SoldierData.maxHPs[_level - 1]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isFighting && collision.gameObject.CompareTag("Rival"))
        {
            _isFighting = true;
            _isFollowRival = false;
            _focusRival = collision.gameObject;
            _navMeshAgent.isStopped = true;
            StartCoroutine(FightRival());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isDead && collision.gameObject.CompareTag("Rival"))
        {
            _isFighting = false;
            _isFollowRival = true;
            _navMeshAgent.isStopped = false;
            _focusRival = null;
        }
    }

    private IEnumerator FightRival()
    {
        RivalID rivalID = _focusRival.GetComponent<RivalID>();

        while (_isFighting && rivalID.GetCurrentHP() > 0 && GetCurrentHP() > 0)
        {
            HitRival(rivalID);

            yield return new WaitForSeconds(_SoldierData.hitCountDowns[_level - 1]);

            SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.idle);
        }

        _isFighting = false;
        _navMeshAgent.isStopped = false;
        _isFollowRival = false;
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
    private void CheckHealth()
    {
        if (_currentHP <= 0 && !_isDead)
        {
            _isDead = true;
            _navMeshAgent.isStopped = true;
            SetBar();
            _soldiers[_level - 1].SetActive(false);
            SoliderSpawnSystem.Instance.DeleteSoldier(_level);
            SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.dead);
            _soldierAnimType = SoldierMoveSystem.SoldierAnimType.idle;
            GameManager.Instance.GridPlacementWrite(GridSystem.Instance.mainGrid);
            gameObject.SetActive(false);
        }
    }
    private void SetBar()
    {
        _barImage.fillAmount = 1;
    }
    private void HitRival(RivalID rivalID)
    {
        SetSoldierAnim(SoldierMoveSystem.SoldierAnimType.attack);

        SoundSystem.Instance.CallSword();
        rivalID.DownHP(_SoldierData.damages[_level - 1]);
    }
    private void FollowRival()
    {
        if (_focusRival != null)
            _navMeshAgent.SetDestination(_focusRival.transform.position);
    }
    private void LevelPlacement(int level)
    {
        _level = level;
    }
    private void SoldierVisibility(int level)
    {
        _soldiers[level - 1].SetActive(true);
    }
    private void HPPlacement(int level)
    {
        _currentHP = _SoldierData.maxHPs[_level - 1];
    }
}
