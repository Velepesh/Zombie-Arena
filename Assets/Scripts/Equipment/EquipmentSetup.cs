using InfimaGames.LowPolyShooterPack;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSetup : MonoBehaviour
{
    [SerializeField, NotNull] private List<Weapon> _weapons;

    private Equipment _model;
    private EquipmentSaver _saver;
    private EquipmentPresenter _presenter;
    private IReadOnlyList<Weapon> _weaponsList => _weapons;

    public Equipment Model => _model;

    public void Init()
    {
        _saver = new EquipmentSaver(_weaponsList);
        _model = new Equipment(_weaponsList);
        _presenter = new EquipmentPresenter(_model, _saver, _weaponsList);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}
