using System;
using System.Collections.Generic;

public interface IZombieSpawner
{
    int WavesCount { get; }
    int ZombiesNumberInWave { get; }

    void StartSpawn();
    void Init(IReadOnlyList<ISpawnPoint> spawnPoints, Level level, ZombieTargetsCompositeRoot targets, bool isMobilePlatform, IWaveSetter waveSetter);
    
    event Action Ended;
    event Action HeadshotReceived;
    event Action BodyshotReceived;
    event Action<Zombie> ZombieDied;
    event Action<int> WaveSetted;
    event Action Loaded;
}