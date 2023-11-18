using UnityEngine;

public class AppStartup : MonoBehaviour
{
    private void Start()
    {
        LoadingScreen.Instance.Load();
    }
}