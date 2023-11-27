using YG;

public class LevelCounterSaver
{
    public LevelCounterSaver()
    {
        Load();
    }
    
    public int Level { get; private set; }

    public void OnLevelIncreased(int level)
    {
        if (Level == level)
            return;

        Level = level;
        Save();
    }

    private void Load()
    {
        Level = YandexGame.savesData.Level;
    }

    private void Save()
    {
        if (Level == YandexGame.savesData.Level)
            return;

        YandexGame.savesData.Level = Level;
        YandexGame.SaveProgress();
    }
}