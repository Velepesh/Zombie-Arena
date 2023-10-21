using UnityEngine;
using YG;

public class ScreenSelector : MonoBehaviour
{
    [SerializeField] private Screen _desktopScreen;
    [SerializeField] private Screen _mobileScreen;

    private void Start()
    {
        if (YandexGame.SDKEnabled == true)
            Load();
    }

    private void Load()
    {
        if (YandexGame.EnvironmentData.isMobile)
            SetMobileScreen();
        else
            SetDesktopScreen();
    }

    private void SetMobileScreen()
    {
        DisableScreen(_desktopScreen);
        EnableScreen(_mobileScreen);
    }

    private void SetDesktopScreen()
    {
        DisableScreen(_mobileScreen);
        EnableScreen(_desktopScreen);
    }

    private void EnableScreen(Screen screen)
    {
        screen.gameObject.SetActive(true);
    }

    private void DisableScreen(Screen screen)
    {
        screen.gameObject.SetActive(false);
    }
}
