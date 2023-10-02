using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Events;

public class WeaponView : ItemView
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private WeaponButton _button;

    public Weapon Weapon => _weapon;

    public event UnityAction<Weapon, WeaponView> Clicked;

    private void OnEnable()
    {
        _weapon.Inited += OnInited;
        _button.Clicked += OnClicked;
    }

    private void OnDisable()
    {
        _weapon.Inited -= OnInited;
        _button.Clicked -= OnClicked;
    }

    public void SelectView()
    {
        _button.SetSelectedColor();
    }

    public void UpdateView()
    {
        if (_weapon.IsEquip)
            _button.SetChoosedColor();
        else if (_weapon.IsUnlock)
            _button.SetUnlockedColor();
        else
            _button.SetDefaultColor();
    }

    private void OnClicked()
    {
        Clicked?.Invoke(_weapon, this);
    }

    private void OnInited()
    {
        SetImages(_weapon);
        SetLabel(_weapon);
        UpdateView();
    }
}