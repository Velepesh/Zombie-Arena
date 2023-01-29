using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private Zombie _zombie;

    public bool CanSpawn => _zombie == null;
    public Vector3 Position => transform.position;

    private void OnDisable()
    {
        if (_zombie != null)
            _zombie.Died -= OnDied;
    }

    public void Init(Zombie zombie)
    {
        _zombie = zombie;
        _zombie.Died += OnDied;
    }


    private void OnDied(IDamageable damageable)
    {
        _zombie.Died -= OnDied;
        _zombie = null;
    }
}
