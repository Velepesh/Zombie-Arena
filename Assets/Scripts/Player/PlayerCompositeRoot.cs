using InfimaGames.LowPolyShooterPack;
using System;
using UnityEngine;
using YG;

public class PlayerCompositeRoot : CompositeRoot
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerSetup _setup;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Character _character;

    readonly private int _grenadeIncreaseThresholdByLevel = 5;

    private LevelCounter _levelCounter;
    private Equipment _equipment;

    public Player Player => _player;

    public void Init(LevelCounter levelCounter, Equipment equipment)
    {
        _levelCounter = levelCounter;
        _equipment = equipment;

        _setup.Init(_player);
    }

    public override void Compose()
    {
        _inventory.Init(_equipment.GetEquipedWeapons());

        int grenadesCount = (_levelCounter.Level / _grenadeIncreaseThresholdByLevel) + 1;
        _character.SetTotalGrenades(grenadesCount);
    }
}

public class PlayerSaver
{
    private Player _player;

    public PlayerSaver(Player player)
    {
        _player = player;
        Load();
    }


    public void Save(int health)
    {
        if (health <= 0)
            throw new ArgumentException(nameof(health));
        
        if (health == YandexGame.savesData.PlayerHealth)
            return;

        YandexGame.savesData.PlayerHealth = health;
        YandexGame.SaveProgress();
    }

    private void Load()
    {
        int health = YandexGame.savesData.PlayerHealth;

        _player.Health.SetHealth(health);
    }
}