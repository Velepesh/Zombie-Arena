using UnityEngine;
using System;

[Serializable]
public class Highscore
{
    [SerializeField] private int _goldScore;
    [SerializeField] private int _silverScore;
    [SerializeField] private int _bronzeScore;

    public void Init(int highscore)
    {
        if (highscore < 0)
            throw new ArgumentException(nameof(highscore));

        Value = highscore;
    }

    public int GoldScore => _goldScore;
    public int SilverScore => _silverScore;
    public int BronzeScore => _bronzeScore;

    public int Value { get; private set; }

    public event Action<int> Recorded;

    public void Record(int highscore)
    {
        if (highscore < Value)
            return;

        Value = highscore;
        Recorded?.Invoke(Value);
    }
}
