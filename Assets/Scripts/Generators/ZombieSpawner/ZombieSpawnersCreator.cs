using System;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnersCreator : MonoBehaviour
{
    [SerializeField] private LevelsHolder _levelsHolder;
    [SerializeField] private ZombiesSpawner _spawnerPrefab;
    [SerializeField] private List<SpawnPoint> _enemySpawnPoints;

    private ZombieTargetsCompositeRoot _targets;
    private bool _isMobilePlatform;

    public ZombiesSpawner CreateSpawner(GameMode gameMode, LevelCounter levelCounter, ZombieTargetsCompositeRoot targets, bool isMobilePlatform)
    {
        _targets = targets;
        _isMobilePlatform = isMobilePlatform;

        if (gameMode == GameMode.Classic)
        {
            return InitSpawner(Instantiate(_spawnerPrefab), _levelsHolder.GetClassicLevel(levelCounter), new ClassicWavesSetter());
        }
        else if (gameMode == GameMode.Infinite)
        {
            InfiniteLevel level = _levelsHolder.InfiniteWavesLevel;
            return InitSpawner(Instantiate(_spawnerPrefab), level, new InfiniteWavesSetter(level.MaxWaveIndex, level.MinCircleWaveIndex));
        }

        throw new ArgumentException(nameof(gameMode));
    }

    private ZombiesSpawner Instantiate(ZombiesSpawner zombieSpawner)
    {
        return Instantiate(zombieSpawner.gameObject, transform.position, Quaternion.identity).GetComponent<ZombiesSpawner>();
    }

    private ZombiesSpawner InitSpawner(ZombiesSpawner zombieSpawner, Level level, IWaveSetter waveSetter)
    {
        zombieSpawner.Init(_enemySpawnPoints, level, _targets, _isMobilePlatform, waveSetter);

        return zombieSpawner;
    }
}
