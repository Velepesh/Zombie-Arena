public interface IWaveSetter
{
    int GetNextWaveIndex(int currentWaveNumber);
    bool CheckForNextWave(int currentWaveNumber, int wavesCount);
}