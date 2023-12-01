using System;

public class Highscore
{
    public Highscore(int highscore)
    {
        if (highscore < 0)
            throw new ArgumentException(nameof(highscore));

        Value = highscore;
    }

    public int Value { get; private set; }

    public event Action<int> Recorded;

    public void Record(int highscore)
    {
        Value = highscore;
        Recorded?.Invoke(Value);
    }
}
