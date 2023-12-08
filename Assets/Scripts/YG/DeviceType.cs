using System.Runtime.InteropServices;
using UnityEngine;

public class DeviceType : MonoBehaviour
{
#if UNITY_EDITOR
    public static bool IsMobileBrowser()
    {
        return false;
    }
#else
    [DllImport("__Internal")]
    public static extern bool IsMobileBrowser();
#endif

    void Awake()
    {
        if (IsMobileBrowser())
        {
            Debug.Log("Открыто на телефоне");
        }
        else
        {
            Debug.Log("Открыто на ПК");
        }
    }
}