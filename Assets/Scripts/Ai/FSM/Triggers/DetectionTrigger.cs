using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DetectionTrigger : StateMachineBehaviour
{
    [SerializeField] private float _delay = 0.5f;
    [SerializeField] private float _angleView;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private LayerMask _layerMask;

    private Transform _transform;
    private TankStateMachine _tank;
    private Agent _target;
    private float _elapsedTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _transform = animator.GetComponentInChildren<Tank>().GetComponent<Transform>();
        _tank = animator.GetComponentInChildren<TankStateMachine>();
        _elapsedTime = Random.Range(0, _delay);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (TimeOver())
        {
            bool isDetected = Scan(out _target);
            animator.SetBool("isDetected", isDetected);

            if (isDetected)
                _tank.Target = _target;
        }
    }

    private bool Scan(out Agent target)
    {
        Collider[] hits = Physics.OverlapSphere(_transform.position, _detectionRadius, _layerMask);
        Dictionary<float, Agent> distances = new Dictionary<float, Agent>();
        target = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Agent aiTarget))
            {
                if (aiTarget.IsDead)
                    continue;

                if (hit.transform == _transform)
                    continue;

                Vector3 direction = hit.transform.position - _transform.position;
                float angle = Vector3.Angle(_transform.forward, direction);

                if (angle > _angleView / 2)
                    continue;

                distances.Add(direction.magnitude, aiTarget);
            }
        }

        if (distances.Count > 0)
            target = distances[distances.Keys.Min()];

        return target != null;
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
}
