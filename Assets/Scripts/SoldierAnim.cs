using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnim : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void PlayIdleAnim()
    {
        _animator.SetBool("idleBool", true);
        _animator.SetBool("runBool", false);
        _animator.SetBool("deadBool", false);
        _animator.SetBool("attackBool", false);
    }
    public void PlayRunAnim()
    {
        _animator.SetBool("idleBool", false);
        _animator.SetBool("runBool", true);
        _animator.SetBool("deadBool", false);
        _animator.SetBool("attackBool", false);
    }
    public void PlayDeadAnim()
    {
        _animator.SetBool("idleBool", false);
        _animator.SetBool("runBool", false);
        _animator.SetBool("deadBool", true);
        _animator.SetBool("AttackBool", false);
    }
    public void PlayAttackAnim()
    {
        _animator.SetBool("idleBool", false);
        _animator.SetBool("runBool", false);
        _animator.SetBool("deadBool", false);
        _animator.SetBool("attackBool", true);
    }

}
