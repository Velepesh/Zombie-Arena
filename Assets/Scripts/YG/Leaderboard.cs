using UnityEngine;
using YG;

public class Leaderboard : MonoBehaviour
{
    [Tooltip("“ехническое название соревновательной таблицы")]
    [SerializeField] private string _nameLB;

    private int _currentScore;

    public void UpdateLeaderboard(int totalScore)
    {
        _currentScore = YandexGame.savesData.Score + totalScore;
        YandexGame.NewLeaderboardScores(_nameLB, _currentScore);

        Save();
    }

    private void Save()
    {
        YandexGame.savesData.Score = _currentScore;
        YandexGame.SaveProgress();
    }
}