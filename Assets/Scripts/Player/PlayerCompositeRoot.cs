using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using YG;

public class PlayerCompositeRoot : Builder
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerViewSetup _setup;
    [SerializeField] private Equipment _equipment;
    [SerializeField] private Inventory _inventory;

    public Player Player => _player;
   
    private void Awake()
    {
        _setup.enabled = false;

        if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
        _player.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
        _player.Died -= OnPlayerDied;
    }

    public override void Compose()
    {
        _setup.enabled = true;
        _inventory.Init(_equipment.GetEquipedWeapons());
    }

    public override void AddHealth(int value)
    {
        _player.Health.AddHealth(value);

        Save();
    }

    private void OnPlayerDied(IDamageable damageable)
    {
        OnDied();
    }

    private void Load()
    {
        int health = YandexGame.savesData.PlayerHealth;

        _player.Health.SetStartHealth(health);
    }

    private void Save()
    {
        if (_player.Health.Value == YandexGame.savesData.PlayerHealth)
            return;

        YandexGame.savesData.PlayerHealth = _player.Health.Value;
        YandexGame.SaveProgress();
    } 
}