using System.Collections.Generic;
using UnityEngine;

public class SpawnersCreator : MonoBehaviour
{
    [SerializeField] private LevelsHolder _levelsHolder;
    [SerializeField] private ZombiesSpawner _zombiesSpawnerPrefab;
    [SerializeField] private GoodsSpawner _goodsSpawnerPrefab;
    [SerializeField] private List<SpawnPoint> _enemySpawnPoints;

    private ZombiesSpawner _zombiesSpawner;
    private GoodsSpawner _goodsSpawner;
    private TargetsCompositeRoot _targets;
    private bool _isMobilePlatform;

    public void RunSpawners()
    {
        _zombiesSpawner.Run();

        if(_goodsSpawner != null)
            _goodsSpawner.Run();
    }

    public ZombiesSpawner CreateZombieSpawner(GameMode gameMode, int levelIndex, TargetsCompositeRoot targets, bool isMobilePlatform)
    {
        _targets = targets;
        _isMobilePlatform = isMobilePlatform;

        if (gameMode == GameMode.Classic)
        {
            _zombiesSpawner = InitSpawner(InstantiateZombiesSpawner(_zombiesSpawnerPrefab), _levelsHolder.GetClassicLevel(levelIndex), new ClassicWavesSetter());
        }
        else if (gameMode == GameMode.Infinite)
        {
            InfiniteLevel level = _levelsHolder.InfiniteWavesLevel;
            _zombiesSpawner = InitSpawner(InstantiateZombiesSpawner(_zombiesSpawnerPrefab), level, new InfiniteWavesSetter(level.MaxWaveIndex, level.MinCircleWaveIndex));
        }

        return _zombiesSpawner;
    }

    public void CreateGoodsSpawner(Health twinsHealth, ZombiesSpawner zombiesSpawner)
    {
        _goodsSpawner = InstantiateGoodsSpawner();
        _goodsSpawner.Init(twinsHealth, zombiesSpawner);
    }


    private ZombiesSpawner InstantiateZombiesSpawner(ZombiesSpawner zombieSpawner)
    {
        return Instantiate(zombieSpawner.gameObject, transform.position, Quaternion.identity).GetComponent<ZombiesSpawner>();
    }

    private ZombiesSpawner InitSpawner(ZombiesSpawner zombieSpawner, Level level, IWaveSetter waveSetter)
    {
        zombieSpawner.Init(_enemySpawnPoints, level, _targets, _isMobilePlatform, waveSetter);

        return zombieSpawner;
    }

    private GoodsSpawner InstantiateGoodsSpawner()
    {
        return Instantiate(_goodsSpawnerPrefab.gameObject, transform.position, Quaternion.identity).GetComponent<GoodsSpawner>();
    }
}
