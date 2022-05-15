using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : StateMachineBehaviour
{
    private TankStateMachine _stateMachine;
    private Agent _agent;
    private Animator _animator;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.GetComponentInChildren<Agent>();
        _stateMachine = animator.GetComponentInChildren<TankStateMachine>();
        _animator = animator;
        _agent.Damaged += OnDamaged;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.Damaged -= OnDamaged;
    }

    private void OnDamaged(Agent enemy)
    {
        if (enemy.IsDead)
            return;

        _stateMachine.Target = enemy;
        _animator.SetTrigger("damaged");
    }
}
