using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTargetTrigger : StateMachineBehaviour
{
    private Animator _animator;
    private Agent _target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _target = animator.GetComponentInChildren<TankStateMachine>().Target;
        _animator = animator;
        _target.Die += OnTargetDie;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _target.Die -= OnTargetDie;
    }

    private void OnTargetDie()
    {
        _animator.SetTrigger("targetIsDead");
    }
}
