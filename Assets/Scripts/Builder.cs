using UnityEngine;

public abstract class Builder : MonoBehaviour
{
    public abstract void Form();
    public abstract void Deactivate();
    public abstract void AddHealth();
}