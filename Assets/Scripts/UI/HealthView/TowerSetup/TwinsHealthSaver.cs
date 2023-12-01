using System;
using YG;

public class TwinsHealthSaver
{
    private Twins _twins;

    public TwinsHealthSaver(Twins twins)
    {
        _twins = twins;
        Load();
    }

    public void Save(int health)
    {
        if (health <= 0)
            throw new ArgumentException(nameof(health));

        if (health == YandexGame.savesData.TwinsHealth)
            return;

        YandexGame.savesData.TwinsHealth = health;
        YandexGame.SaveProgress();
    }

    private void Load()
    {
        int health = YandexGame.savesData.TwinsHealth;

        _twins.Health.SetHealth(health);
    }
}