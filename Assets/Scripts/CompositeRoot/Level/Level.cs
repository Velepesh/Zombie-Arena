using System.Collections.Generic;
using UnityEngine;

public abstract class Level : ScriptableObject
{
    [SerializeField] private List<Wave> _waves;

    protected IReadOnlyList<IWave> Waves => _waves;

    public int GetWavesCount(ILevel level)
    {
        return level.WavesCount;
    }

    public Wave GetWave(int index)
    {
       return _waves[index];
    }
}