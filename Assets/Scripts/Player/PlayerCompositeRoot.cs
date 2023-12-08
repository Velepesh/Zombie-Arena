using System;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class PlayerCompositeRoot : CompositeRoot
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerSetup _setup;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Character _character;

    private int _currentLevel;
    private GameMode _gameMode;
    private Equipment _equipment;

    public Player Player => _player;

    public void Init(int currentLevel, Equipment equipment, bool isMobile)
    {
        if (currentLevel <= 0)
            throw new ArgumentException(nameof(currentLevel));

        if (equipment == null)
            throw new ArgumentNullException(nameof(equipment));

        _currentLevel = currentLevel;
        _equipment = equipment;

        _setup.Init(_player);
        _character.SetPlatform(isMobile);
    }

    public override void Compose()
    {
        _inventory.Init(_equipment.GetEquipedWeapons());

        _character.AddGrenadesByLevel(_currentLevel, _gameMode);
    }

    public void OnGameModeSelected(GameMode gameMode)
    {
        _gameMode = gameMode;
    }
}