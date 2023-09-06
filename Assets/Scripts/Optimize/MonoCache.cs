using System.Collections.Generic;
using UnityEngine;

public class MonoCache : MonoBehaviour
{
    public static List<MonoCache> AllUpdate = new List<MonoCache>(10001);
    public static List<MonoCache> AllFixedUpdate = new List<MonoCache>(10001);
    public static List<MonoCache> AllLateUpdate = new List<MonoCache>(10001);

    private void OnDestroy()
    {
        RemoveUpdate();
    }
    
    protected void AddUpdate() => AllUpdate.Add(this);
    protected void RemoveUpdate() => AllUpdate.Remove(this);
    protected void AddFixedUpdate() => AllFixedUpdate.Add(this);
    protected void AddLateUpdate() => AllLateUpdate.Remove(this);
    protected void RemoveFixedUpdate() => AllFixedUpdate.Remove(this);
    protected void RemoveLateUpdate() => AllLateUpdate.Remove(this);

    public void Tick() => OnTick();
    public void FixedTick() => OnFixedTick();
    public void LateTick() => OnLateTick();

    public virtual void OnTick() { }
    public virtual void OnFixedTick() { }
    public virtual void OnLateTick() { }
}