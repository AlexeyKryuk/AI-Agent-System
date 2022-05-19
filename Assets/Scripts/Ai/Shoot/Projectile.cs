using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _curvature = 0.2f;

    private Rigidbody _rigidbody;
    private Agent _owner;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == _rigidbody)
            return;

        if (other.TryGetComponent(out Agent target))
        {
            target.ApplyDamage(_owner, _damage);
            Debug.Log("applydamage");
            Destroy(gameObject);
        }
    }

    public void Cast(Agent owner, Vector3 target)
    {
        _owner = owner;
        StartCoroutine(Casting(target));
    }

    private IEnumerator Casting(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            Vector3 from = transform.forward;
            Vector3 to = target - transform.position;
            float time = _curvature / Vector3.Distance(target, transform.position);

            transform.forward = Vector3.Slerp(from, to, time);
            _rigidbody.position += transform.forward * _moveSpeed * Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Destroy");
        Destroy(gameObject);
    }
}
