using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStateMachine : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Agent _agent;

    private Agent _target;
    private Agent _lastTarget;

    public Agent Target
    {
        get
        {
            if (HasTarget)
                return _target;
            else
                return _lastTarget;
        }
        set
        {
            HasTarget = value != null;

            if (!HasTarget)
                _lastTarget = _target;

            _target = value;
        }
    }

    public bool HasTarget { get; private set; }

    private void Awake()
    {
        _animator.SetInteger("healthReserve", _agent.HealthReserve);
    }

    private void OnEnable()
    {
        _agent.Die += OnDie;
        _agent.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        _agent.Die -= OnDie;
        _agent.Damaged -= OnDamaged;
    }

    private void Update()
    {
        if (HasTarget)
        {
            float distance = Vector3.Distance(Target.Transform.position, transform.position);
            _animator.SetFloat("distanceToTarget", distance);
        }
    }

    private void OnDie()
    {
        _animator.SetTrigger("isDead");
    }

    private void OnDamaged(Agent enemy)
    {
        _animator.SetInteger("healthReserve", _agent.HealthReserve);
        _animator.SetTrigger("damaged");
    }
}
