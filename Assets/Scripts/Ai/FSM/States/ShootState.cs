using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : State
{
    [SerializeField] private float _recharge;
    [SerializeField] private Projectile _projectile;

    private Transform _muzzle;
    private Agent _target;
    private Agent _own;
    private Movement _movement;
    private float _elapsedTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _target = animator.GetComponentInChildren<TankStateMachine>().Target;
        _own = animator.GetComponentInChildren<Tank>();
        _movement = animator.GetComponentInChildren<Movement>();
        _muzzle = animator.GetComponentInChildren<Muzzle>().transform;
        _elapsedTime = Random.Range(0, _recharge);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_target.IsDead)
            return;

        _movement.Rotate(_target.Transform.position);

        if (_elapsedTime >= _recharge)
        {
            Shoot();
            _elapsedTime = 0;
        }

        _elapsedTime += Time.deltaTime;
    }

    private void Shoot()
    {
        var projectile = Instantiate(_projectile, _muzzle.position, _muzzle.rotation);
        projectile.Cast(_own, _target.Transform.position);
    }
}
