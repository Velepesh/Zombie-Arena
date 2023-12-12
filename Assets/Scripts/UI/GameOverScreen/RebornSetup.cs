using UnityEngine;
using System;

public class RebornSetup : MonoBehaviour 
{
    private Player _player;
    private Twins _twins;

    public void Init(Player player, Twins twins)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        if (twins == null)
            throw new ArgumentNullException(nameof(twins));

        _player = player;
        _twins = twins;
    }

    public void Reborn()
    {
        _player.Health.Reborn();
        _twins.Health.Reborn();
    }
}