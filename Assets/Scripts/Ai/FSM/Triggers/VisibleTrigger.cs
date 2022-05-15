using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleTrigger : StateMachineBehaviour
{
    [SerializeField] private float _delay = 0.5f;

    private float _elapsedTime;
    private float _searchTime;
    private Transform _transform;
    private TankStateMachine _stateMachine;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.GetComponentInChildren<Tank>().GetComponent<Transform>();
        _stateMachine = animator.GetComponentInChildren<TankStateMachine>();
        _elapsedTime = Random.Range(0, _delay);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (TimeOver())
        {
            if (_stateMachine.HasTarget && !_stateMachine.Target.IsDead)
            {
                Collider target = _stateMachine.Target.Collider;
                Vector3 direction = target.transform.position - _transform.position;
                bool visible = CanSee(target, direction);

                animator.SetFloat("searchTime", SearchTarget(visible));
                animator.SetBool("isVisible", visible);
            }
            else
            {
                animator.SetFloat("searchTime", SearchTarget(false));
                animator.SetBool("isVisible", false);
            }
        }
    }

    private bool CanSee(Collider target, Vector3 direction)
    {
        Ray ray = new Ray(_transform.position, direction);
        float radius = target.bounds.max.y - target.bounds.center.y;

        Physics.SphereCast(ray, radius, out RaycastHit hit);

        return hit.collider == target;
    }

    private bool TimeOver()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _delay)
        {
            _elapsedTime = 0;
            return true;
        }
        return false;
    }

    private float SearchTarget(bool visible)
    {
        _searchTime += Time.deltaTime + _delay;

        if (visible)
            _searchTime = 0;

        return _searchTime;
    }
}
