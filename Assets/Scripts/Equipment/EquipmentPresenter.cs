using InfimaGames.LowPolyShooterPack;
using System.Collections.Generic;

public class EquipmentPresenter
{
    private Equipment _model;
    private EquipmentSaver _saver;
    private IReadOnlyList<Weapon> _weapons;

    public EquipmentPresenter(Equipment model, EquipmentSaver saver, IReadOnlyList<Weapon> weapons)
    {
        _model = model;
        _saver = saver;
        _weapons = weapons;
    }

    public void Enable()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            weapon.Bought += OnBought;
            weapon.Equiped += OnEquiped;
        }
    }

    public void Disable()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            weapon.Bought -= OnBought;
            weapon.Equiped -= OnEquiped;
        }
    }

    private void OnBought(Weapon weapon)
    {
        _saver.OnBought(weapon);
    }

    private void OnEquiped(Weapon weapon)
    {
        _saver.OnEquiped(weapon);
    }
}
