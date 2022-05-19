using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private float _moveSpeed;

    private Movement _movement;
    private Agent _target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _movement = animator.GetComponentInChildren<Tank>().GetComponent<Movement>();
        _target = animator.GetComponentInChildren<TankStateMachine>().Target;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_target != null)
            _movement.Move(_target.Transform.position, _moveSpeed);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movement.StopMove();
    }
}
