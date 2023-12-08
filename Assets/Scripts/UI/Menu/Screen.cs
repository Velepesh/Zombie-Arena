using UnityEngine;

public class Screen : MonoBehaviour
{
    public void ShowScreen()
    {
        ObjectEnabler.Enable(gameObject);
    }

    public void DisableScreen()
    {
        ObjectEnabler.Disable(gameObject);
    }
}
