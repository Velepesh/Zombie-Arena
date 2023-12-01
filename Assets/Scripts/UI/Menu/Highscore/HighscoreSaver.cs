using YG;

public class HighscoreSaver
{
    public HighscoreSaver()
    {
        Load();
    }

    public int Highscore { get; private set; }

    public void OnRecorded(int highscore)
    {
        if (Highscore == highscore)
            return;

        Highscore = highscore;
        Save();
    }

    private void Load()
    {
        Highscore = YandexGame.savesData.Highscore;
    }

    private void Save()
    {
        if (Highscore == YandexGame.savesData.Highscore)
            return;

        YandexGame.savesData.Highscore = Highscore;
        YandexGame.SaveProgress();
    }
}