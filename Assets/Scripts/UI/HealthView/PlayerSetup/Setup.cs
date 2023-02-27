using UnityEngine;

public abstract class Setup : MonoBehaviour
{
    protected abstract void Awake();
    protected abstract void OnEnable();
    protected abstract void OnDisable();
}