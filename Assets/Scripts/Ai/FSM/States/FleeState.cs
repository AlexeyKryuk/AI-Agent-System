using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class FleeState : StateMachineBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _fieldOfView = 90f;
    [SerializeField] private float _radius = 12f;

    [SerializeField] private LayerMask _hidableLayers;
    [SerializeField] private LayerMask _lineOfSightLayers;

    [Range(1, 20)] 
    [SerializeField] private float _minPlayerDistance = 5f;

    [Range(0, 5f)]
    [SerializeField] private float _minObstacleHeight = 1.25f;

    [Range(-1, 1)]
    [Tooltip("Lower is a better hiding spot")]
    [SerializeField] private float _hideSensitivity = 0;

    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private Agent _agent;
    private Agent _target;

    private Collider[] Colliders = new Collider[15];

    private float _elapsedTime;
    private float _delay = 0.25f;
    private float _initialSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.GetComponentInChildren<Tank>().GetComponent<Transform>();
        _target = animator.GetComponentInChildren<TankStateMachine>().Target;
        _navMeshAgent = animator.GetComponentInChildren<NavMeshAgent>();
        _agent = animator.GetComponentInChildren<Agent>();

        _agent.Damaged += OnDamaged;
        _initialSpeed = _navMeshAgent.speed;
        _navMeshAgent.speed = _moveSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (TimeOver())
            CheckLineOfSight(_target.transform);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _navMeshAgent.speed = _initialSpeed;
        _agent.Damaged -= OnDamaged;
    }

    private void OnDamaged(Agent agent)
    {
        _target = agent;
        Hide(agent.transform);
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

    private void CheckLineOfSight(Transform Target)
    {
        Vector3 direction = (Target.transform.position - _transform.position).normalized;
        float dotProduct = Vector3.Dot(_transform.forward, direction);

        float distance = Vector3.Distance(Target.transform.position, _transform.position);

        if (dotProduct >= Mathf.Cos(_fieldOfView))
        {
            if (Physics.Raycast(_transform.position, direction, out RaycastHit hit, 1000f, _lineOfSightLayers))
                Hide(Target);
            else if (distance < _minPlayerDistance)
                Hide(Target);
        }
        else if (distance < _minPlayerDistance)
            Hide(Target);
    }

    private void Hide(Transform Target)
    {
        for (int i = 0; i < Colliders.Length; i++)
        {
            Colliders[i] = null;
        }

        int hits = Physics.OverlapSphereNonAlloc(_navMeshAgent.transform.position, _radius, Colliders, _hidableLayers);

        int hitReduction = 0;
        for (int i = 0; i < hits; i++)
        {
            if (Vector3.Distance(Colliders[i].transform.position, Target.position) < _minPlayerDistance || Colliders[i].bounds.size.y < _minObstacleHeight)
            {
                Colliders[i] = null;
                hitReduction++;
            }
        }
        hits -= hitReduction;

        System.Array.Sort(Colliders, ColliderArraySortComparer);

        for (int i = 0; i < hits; i++)
        {
            if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 50f, _navMeshAgent.areaMask))
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, _navMeshAgent.areaMask))
                {
                    Debug.LogError($"Unable to find edge close to {hit.position}");
                }

                if (Vector3.Dot(hit.normal, (Target.position - hit.position).normalized) < _hideSensitivity)
                {
                    _navMeshAgent.SetDestination(hit.position);
                    break;
                }
                else
                {
                    // Since the previous spot wasn't facing "away" enough from teh target, we'll try on the other side of the object
                    if (NavMesh.SamplePosition(Colliders[i].transform.position - (Target.position - hit.position).normalized * 4, out NavMeshHit hit2, 50f, _navMeshAgent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hit2.position, out hit2, _navMeshAgent.areaMask))
                        {
                            Debug.LogError($"Unable to find edge close to {hit2.position} (second attempt)");
                        }

                        if (Vector3.Dot(hit2.normal, (Target.position - hit2.position).normalized) < _hideSensitivity)
                        {
                            _navMeshAgent.SetDestination(hit2.position);
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"Unable to find NavMesh near object {Colliders[i].name} at {Colliders[i].transform.position}");
            }
        }
    }

    public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return 
                Vector3.Distance(_navMeshAgent.transform.position, A.transform.position)
                .CompareTo(
                Vector3.Distance(_navMeshAgent.transform.position, B.transform.position));
        }
    }
}
