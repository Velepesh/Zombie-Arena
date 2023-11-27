using System;

public class LevelCounter
{
    public LevelCounter(int level)
    {
        if (level <= 0)
            throw new ArgumentException(nameof(level));

        Level = level;
    }

    public int Level { get; private set; }

    public event Action<int> LevelIncreased;

    public void IncreaseLevel()
    {
        Level++;
        LevelIncreased?.Invoke(Level);
    }
}