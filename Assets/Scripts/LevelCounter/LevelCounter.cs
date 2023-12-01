using System;

public class LevelCounter
{
    public LevelCounter(int level)
    {
        if (level <= 0)
            throw new ArgumentException(nameof(level));

        Index = level;
    }

    public int Index { get; private set; }

    public event Action<int> LevelIncreased;

    public void IncreaseLevel()
    {
        Index++;
        LevelIncreased?.Invoke(Index);
    }
}