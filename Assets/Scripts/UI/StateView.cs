using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateView : MonoBehaviour
{
    [SerializeField] private TankStateMachine _stateMachine;
    [SerializeField] private TMP_Text _text;

    private void Update()
    {
        _text.text = _stateMachine.CurrentStateName;
    }
}