using UnityEngine;
using YG;
using YG.Utils.LB;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private ScoreSetup _scoreSetup;

    [Tooltip("“ехническое название соревновательной таблицы")]
    [SerializeField] private string _nameLB;

    [Tooltip("ћаксимальное кол-во получаемых игроков")]
    [SerializeField] private int _maxQuantityPlayers = 20;

    [Tooltip(" ол-во получени€ верхних топ игроков")]
    [Range(1, 20)]
    [SerializeField] private int _quantityTop = 3;

    [Tooltip(" ол-во получаемых записей возле пользовател€")]
    [Range(1, 10)]
    [SerializeField] private int _quantityAround = 6;

    readonly private string photoSize = "small";
   
    private int _currentScore;

    private void Awake()
    {
        if (YandexGame.initializedLB)
            UpdateLB();
    }

    private void OnEnable()
    {
        YandexGame.onGetLeaderboard += OnUpdateLB;
        _game.Won += OnWon;
    }

    private void OnDisable()
    {
        YandexGame.onGetLeaderboard -= OnUpdateLB;
        _game.Won -= OnWon;
    }

    void OnUpdateLB(LBData lb)
    {
        var player = lb.thisPlayer;
        _currentScore = player.score;

        UpdateLB();
    }

    public void UpdateLB()
    {
        YandexGame.GetLeaderboard(_nameLB, _maxQuantityPlayers, _quantityTop, _quantityAround, photoSize);
    }

    private void OnWon()
    {
        int newScore = _currentScore + _scoreSetup.Score.TotalScore;
        YandexGame.NewLeaderboardScores(_nameLB, newScore);
    }
}