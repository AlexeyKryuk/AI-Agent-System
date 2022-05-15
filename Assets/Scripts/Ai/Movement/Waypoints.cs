using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private const float _maxDistanceToPoint = 1f;

    [SerializeField] private Transform _agent;

    private List<Transform> _points = new List<Transform>();
    private int _current;

    private void Awake()
    {
        InitializePoints();
    }

    private void Update()
    {
        CheckDistance();
    }

    public Vector3 GetCurrent() => _points[_current].position;

    private void InitializePoints()
    {
        foreach (var point in GetComponentsInChildren<Transform>())
        {
            if (point != this.transform)
                _points.Add(point);
        }
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(_agent.position, _points[_current].position);

        if (distance <= _maxDistanceToPoint)
            _current++;

        if (_current >= _points.Count)
            _current = 0;
    }
}
