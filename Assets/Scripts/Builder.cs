﻿using UnityEngine.Events;

public abstract class Builder : CompositeRoot
{
    public abstract void AddHealth(int value);

    public event UnityAction Died;

    protected void OnDied()
    {
        Died?.Invoke();
    }
}