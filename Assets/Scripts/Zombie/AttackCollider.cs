using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackCollider : MonoBehaviour
{
    [SerializeField] private int _damage;

    private Collider _collider;

    private void OnValidate()
    {
        _damage = Mathf.Clamp(_damage, 0, int.MaxValue);
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        StopAttack();
    }

    public void Attack()
    {
        EnableCollider();
    }

    public void StopAttack()
    {
        DisableCollider();
    }

    private void EnableCollider()
    {
        _collider.enabled = true;
    }

    private void DisableCollider()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ITarget target))
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage, Vector3.zero);
    }
}