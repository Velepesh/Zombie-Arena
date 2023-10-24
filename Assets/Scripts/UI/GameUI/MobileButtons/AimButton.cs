using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class AimButton : ToggleButton
{
    [SerializeField] private Character _character;

    private bool _isAiming = false;

    private void OnEnable()
    {
        IconButtonChanger.OnButtonClicked += OnButtonClicked;
    }

    private void OnDisable()
    {
        IconButtonChanger.OnButtonClicked -= OnButtonClicked;
    }

    private void OnButtonClicked()
    {
        bool isAiming = _character.IsAiming();

        if (_isAiming != isAiming)
        {
            IconButtonChanger.SwitchIcon();
            _isAiming = isAiming;
        }
    }
}