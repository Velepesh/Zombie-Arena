public class ClassicWavesSetter : IWaveSetter
{
    public int GetNextWaveIndex(int currentWaveNumber)
    {
        ++currentWaveNumber;

        return currentWaveNumber;
    }

    public bool CheckForNextWave(int currentWaveNumber, int wavesCount)
    {
        if (currentWaveNumber + 1 >= wavesCount)
            return false;
        
        return true;
    }
}