public class InfiniteWavesSetter : IWaveSetter
{
    private int _maxWaveIndex;
    private int _minCircleWaveIndex;

    public InfiniteWavesSetter(int maxWaveIndex, int minCircleWaveIndex)
    {
        _maxWaveIndex = maxWaveIndex;
        _minCircleWaveIndex = minCircleWaveIndex;
    }

    public int GetNextWaveIndex(int currentWaveNumber)
    {
        if (currentWaveNumber + 1 > _maxWaveIndex)
            currentWaveNumber = _minCircleWaveIndex;
        else
            ++currentWaveNumber;

        return currentWaveNumber;
    }

    public bool CheckForNextWave(int currentWaveNumber, int wavesCount)
    {
        return true;
    }
}
