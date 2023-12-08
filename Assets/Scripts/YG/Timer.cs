using UnityEngine;

public class Timer : MonoCache
{
    [SerializeField] private Game _game;

    private float _spentTime = 0f;
    private bool _isPlaying;

    public int SpentTime => (int)_spentTime;

    private void OnEnable()
    {
        _game.Started += OnStarted;
        _game.Continued += OnContinued;
        _game.Won += OnWon;
        _game.Ended += OnEnded;
        
        AddUpdate();
    }

    private void OnDisable()
    {
        _game.Started -= OnStarted;
        _game.Continued -= OnContinued;
        _game.Won -= OnWon;
        _game.Ended -= OnEnded;

        RemoveUpdate();
    }

    public override void OnTick()
    {
        if (_isPlaying == false)
            return;

        _spentTime += Time.deltaTime;
    }

    private void OnStarted()
    {
        StartTimer();
    }

    private void OnContinued()
    {
        StartTimer();
    }

    private void OnWon()
    {
        StopTimer();
    }

    private void OnEnded()
    {
        StopTimer();
    }


    private void StartTimer()
    {
        _isPlaying = true;
    }

    private void StopTimer()
    {
        _isPlaying = false;
    }
}