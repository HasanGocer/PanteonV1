using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RivalMove : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] RivalID _rivalID;
     GameObject _target;

    public void NavmeshStoped()
    {
        _navMeshAgent.isStopped = true;
    }
    public void NavmeshResume()
    {
        _navMeshAgent.isStopped = false;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        SetAgentPosition();
    }
    private void SetAgentPosition()
    {
        _navMeshAgent.SetDestination(_target.transform.position);
    }
}
