using System;
using UnityEngine;

[Serializable]
public class Health
{
    [SerializeField] private int _maxValue;
    [SerializeField] private int _value;

    public event Action Die;

    public int Value => _value;
    public int MaxValue => _maxValue;

    public void ApplyDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        _value = Mathf.Max(_value - damage, 0);

        if (_value <= 0)
            Die?.Invoke();
    }
}
