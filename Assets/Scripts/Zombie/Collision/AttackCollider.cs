using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class AttackCollider : MonoBehaviour
{
    [SerializeField] private int _damage;

    private Collider _collider;
    private Zombie _zombie;

    public event UnityAction Hit;
    public event UnityAction Attacked;

    private void OnValidate()
    {
        _damage = Mathf.Clamp(_damage, 0, int.MaxValue);
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        DisableCollider();
    }

    public void Init(Zombie zombie)
    {
        _zombie = zombie;
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    public void EnableCollider()
    {
        _collider.enabled = true;

        Attacked?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ITarget target))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                if (damageable is Player player)
                {
                    Hit?.Invoke();
                    player.SetAttackingZombie(_zombie);
                }

                damageable.TakeDamage(_damage, Vector3.zero);
            }
        }
    }
}