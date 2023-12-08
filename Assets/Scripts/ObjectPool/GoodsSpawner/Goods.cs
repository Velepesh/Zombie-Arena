using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Goods : MonoBehaviour
{
    public abstract event Action<Goods> PickedUp;
    public abstract void Init(ISpawnPoint spawnPoint);
    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void Rotate(float durationTime);
}