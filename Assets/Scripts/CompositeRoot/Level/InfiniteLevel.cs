using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "Level/Infinite Level", order = 51)]

public class InfiniteLevel : Level, ILevel
{
    [SerializeField] private int _maxWaveIndex;
    [SerializeField] private int _minCircleWaveIndex;

    public int WavesCount => int.MaxValue;
    public int MaxWaveIndex => _maxWaveIndex;
    public int MinCircleWaveIndex => _minCircleWaveIndex;
}