using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Agent _agent;
    [SerializeField] private LayerMask _enemyLayers;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, float.MaxValue, _enemyLayers))
            {
                if (hit.collider.TryGetComponent(out Agent agent))
                    agent.ApplyDamage(_agent, 1);
            }
        }
    }
}
