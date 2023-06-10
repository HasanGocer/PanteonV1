using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTime : MonoBehaviour
{
    private float _timer = 0f;
    private bool _isStart;
    private float _countDown;
    private float _hitSpeed;
    private int _damage;
    private GameObject _startPos;

    public void SetData(float countdown, float hitSpeeed, int damage, GameObject startPos)
    {
        _damage = damage;
        _hitSpeed = hitSpeeed;
        _countDown = countdown;
        _startPos = startPos;
        _isStart = true;
    }
    public void HitTimeOff()
    {
        _damage = 0;
        _hitSpeed = 0;
        _countDown = 0;
        _startPos = null;
        _isStart = false;
    }

    private void Update()
    {
        if (GameManager.Instance.gameStat == GameManager.GameStat.start && _isStart)
            if (RivalSpawnSystem.Instance.rivals.Count > 0)
            {
                _timer += Time.deltaTime;

                if (_timer >= _countDown)
                {
                    GameObject focusRival = Search();
                    GameObject hit = HitSystem.Instance.GetHit(_startPos.transform.position);
                    hit.transform.LookAt(focusRival.transform);
                    MoveMechanics.Instance.MoveStabile(hit, focusRival, (int)_hitSpeed, ref _isStart, () => Shot(focusRival, hit));
                    _timer = 0;
                }
            }
    }

    private GameObject Search()
    {
        GameObject tempRival = RivalSpawnSystem.Instance.rivals[0];

        for (int i = 1; i < RivalSpawnSystem.Instance.rivals.Count; i++)
            if (CheckDistance(tempRival, RivalSpawnSystem.Instance.rivals[i]))
                tempRival = RivalSpawnSystem.Instance.rivals[i];

        return tempRival;
    }
    private bool CheckDistance(GameObject tempBuild, GameObject newBuild)
    {
        if (Vector3.Distance(tempBuild.transform.position, transform.position) > Vector3.Distance(newBuild.transform.position, transform.position)) return true;
        else return false;
    }
    private void Shot(GameObject focusRival, GameObject hit)
    {
        RivalID rivalID = focusRival.GetComponent<RivalID>();

        rivalID.DownHP(_damage);
        hit.SetActive(false);
    }
}
