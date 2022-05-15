using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private LayerMask _floorLayers;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, float.MaxValue, _floorLayers))
            {
                _agent.SetDestination(hit.point);
            }
        }
    }
}
