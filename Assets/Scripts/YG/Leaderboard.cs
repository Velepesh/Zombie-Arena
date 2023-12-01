using UnityEngine;
using YG;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private ScoreSetup _scoreSetup;

    [Tooltip("“ехническое название соревновательной таблицы")]
    [SerializeField] private string _nameLB;

    private int _currentScore;

    private void OnEnable()
    {
        _game.Won += OnWon;
    }

    private void OnDisable()
    {
        _game.Won -= OnWon;
    }

    private void OnWon()
    {
        _currentScore = YandexGame.savesData.Score + _scoreSetup.TotalScore;
        YandexGame.NewLeaderboardScores(_nameLB, _currentScore);

        Save();
    }

    private void Save()
    {
        YandexGame.savesData.Score = _currentScore;
        YandexGame.SaveProgress();
    }
}