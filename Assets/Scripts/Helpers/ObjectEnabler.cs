using UnityEngine;

public static class ObjectEnabler 
{
    public static void Enable(GameObject go)
    {
        go.SetActive(true);
    }

    public static void Disable(GameObject go)
    {
        go.SetActive(false);
    }
}