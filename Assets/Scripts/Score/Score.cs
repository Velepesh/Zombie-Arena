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
        int award = zombie.Options.Award;

        if (zombie.IsHeadKill)
            return award * 2;

        return award;
    }
}