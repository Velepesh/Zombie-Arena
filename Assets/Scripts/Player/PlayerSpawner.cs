using UnityEngine;
using System;

public class PlayerSpawner : ObjectPool
{
    [SerializeField] private Player _prefab;
    [SerializeField] private SpawnPoint _spawnPoint;

    public Player Player { get; private set; }

    public override void StartGenerate()
    {
        SpawnPrefab(_prefab.gameObject);

        if (TryGetObject(out GameObject player))
            Spawn(player);
        else
            throw new ArgumentNullException(nameof(player));
    }

    private void Spawn(GameObject playerObject)
    {
        if (playerObject.TryGetComponent(out Player player))
        {
            playerObject.SetActive(true);
            playerObject.transform.SetParent(null);
            playerObject.transform.position = _spawnPoint.Position;
            Player = player;
        }
    }
}
