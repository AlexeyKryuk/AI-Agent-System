using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    public void Move(Vector3 position, float speed)
    {
        Vector3 to = new Vector3(position.x, transform.position.y, position.z);
        _agent.destination = to;
    }

    public void MoveTranslate(Vector3 direction)
    {
        transform.Translate(direction * Time.deltaTime);
    }

    public void Rotate(Vector3 to)
    {
        Vector3 from = transform.position;
        Vector3 direction = to - from;
        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10f * Time.deltaTime);
    }

    public void StopMove()
    {
        _agent.destination = transform.position;
    }
}
