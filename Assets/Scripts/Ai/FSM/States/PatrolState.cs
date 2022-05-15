using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : StateMachineBehaviour
{
    [SerializeField] private float _moveSpeed;
    
    private Waypoints _waypoints;
    private Movement _movement;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movement = animator.GetComponentInChildren<Tank>().GetComponent<Movement>();
        _waypoints = animator.GetComponentInChildren<Waypoints>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movement.Move(_waypoints.GetCurrent(), _moveSpeed);
    }
}
