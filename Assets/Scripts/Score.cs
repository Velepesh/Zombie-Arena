using UnityEngine.Events;

public class Score
{
    public int TotalScore { get; private set; }

    public event UnityAction<int> Added;

    public void AddScore(Zombie zombie)
    {
        int value = GetScore(zombie);
        TotalScore += value;
        Added?.Invoke(value);
    }

    private int GetScore(Zombie zombie)
    {
        if (zombie.WasHeadHit)
            return zombie.Options.MaxAward;

        return zombie.Options.MaxAward / 2;
    }
}