using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class CrouchButton : ToggleButton
{
    [SerializeField] private Character _character;

    private bool _isCrouching = false;

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
        bool isCrouching = _character.IsCrouching();

        if (_isCrouching != isCrouching)
        {
            IconButtonChanger.SwitchIcon();
            _isCrouching = isCrouching;
        }
    }
}