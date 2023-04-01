using InfimaGames.LowPolyShooterPack;
using UnityEngine;

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
    }

    public override void Compose()
    {
        _setup.enabled = true;
        _inventory.Init(_equipment.GetEquipedWeapons());
    }

    public override void AddHealth(int value)
    {
        _player.Health.AddHealth(value);
    }
}