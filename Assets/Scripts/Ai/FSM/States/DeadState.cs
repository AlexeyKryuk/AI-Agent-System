using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    private Movement _movement;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _movement = animator.GetComponentInChildren<Tank>().GetComponent<Movement>();
        _movement.StopMove();
    }
}
