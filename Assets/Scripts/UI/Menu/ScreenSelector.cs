using UnityEngine;

public class ScreenSelector : MonoBehaviour
{
    [SerializeField] private Screen _desktopScreen;
    [SerializeField] private Screen _mobileScreen;

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

    private void EnableScreen(Screen screen)
    {
        screen.ShowScreen();
    }

    private void DisableScreen(Screen screen)
    {
        screen.DisableScreen();
    }
}