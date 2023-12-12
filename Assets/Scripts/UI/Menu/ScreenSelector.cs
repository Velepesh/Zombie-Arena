using UnityEngine;

public class ScreenSelector : MonoBehaviour
{
    [SerializeField] private PlatformScreen _desktopScreen;
    [SerializeField] private PlatformScreen _mobileScreen;

    public void Init(bool isMobile)
    {
        SelectScreen(isMobile);
    }

    private void SelectScreen(bool isMobile)
    {
        if (isMobile)
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

    private void EnableScreen(PlatformScreen screen)
    {
        screen.ShowScreen();
    }

    private void DisableScreen(PlatformScreen screen)
    {
        screen.DisableScreen();
    }
}