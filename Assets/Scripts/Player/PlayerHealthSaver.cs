using System;
using YG;

public class PlayerHealthSaver
{
    private Player _player;

    public PlayerHealthSaver(Player player)
    {
        _player = player;
        Load();
    }


    public void Save(int health)
    {
        if (health <= 0)
            throw new ArgumentException(nameof(health));
        
        if (health == YandexGame.savesData.PlayerHealth)
            return;

        YandexGame.savesData.PlayerHealth = health;
        YandexGame.SaveProgress();
    }

    private void Load()
    {
        int health = YandexGame.savesData.PlayerHealth;

        _player.Health.SetHealth(health);
    }
}