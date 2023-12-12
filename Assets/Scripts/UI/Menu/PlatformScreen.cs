using UnityEngine;

public class PlatformScreen : MonoBehaviour
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
