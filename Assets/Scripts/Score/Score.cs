using System;
using UnityEngine.Events;

public class Score
{
    public int TotalScore { get; private set; }

    public event UnityAction<int> Added;

    public void AddScore(Zombie zombie)
    {
        if (zombie == null)
            throw new ArgumentNullException(nameof(zombie));

        int score = Calculate(zombie);
        TotalScore += score;
        Added?.Invoke(score);
    }

    private int Calculate(Zombie zombie)
    {
        int award = zombie.Options.Award;

        if (zombie.IsHeadKill)
            return award * 2;

        return award;
    }
}