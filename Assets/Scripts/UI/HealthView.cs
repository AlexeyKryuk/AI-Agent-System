using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Agent _agent;
    [SerializeField] private Slider _slider;

    private void Update()
    {
        _slider.value = (float)_agent.HealthReserve / 100f;
    }
}
