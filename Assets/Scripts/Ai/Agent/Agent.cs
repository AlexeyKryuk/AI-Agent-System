using System;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Collider))]
public abstract class Agent : MonoBehaviour
{
    [SerializeField] private Health _health;

    public event Action Die;
    public event Action<Agent> Damaged;

    public Transform Transform { get; private set; }
    public Collider Collider { get; private set; }
    public Agent Target { get; private set; }

    public bool IsDead => _health.Value <= 0;
    public int HealthReserve => (int)((float)_health.Value / (float)_health.MaxValue * 100f);

    private void Awake()
    {
        Transform = GetComponent<Transform>();
        Collider = GetComponent<Collider>();
        Target = GetComponent<TankStateMachine>().Target;
    }

    private void OnEnable()
    {
        _health.Die += OnDie;
    }

    private void OnDisable()
    {
        _health.Die -= OnDie;
    }

    public virtual void ApplyDamage(Agent enemy, int damage)
    {
        _health.ApplyDamage(damage);
        Damaged?.Invoke(enemy);
    }

    public virtual void OnDie()
    {
        Die?.Invoke();
    }
}
