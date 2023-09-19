using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TwinCollider : MonoBehaviour
{
    public event UnityAction<int, Vector3> Damaged;

    public void TakeDamage(int damage, Vector3 normal)
    {
        Damaged?.Invoke(damage, normal);
    }
}