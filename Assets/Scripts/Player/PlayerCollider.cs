using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerCollider : MonoBehaviour
{
    public Collider Collider { get; private set; }

    private void Start()
    {
        Collider = GetComponent<Collider>();
    }
}