using TMPro;
using UnityEngine;

public class Timer : MonoCache
{
    [SerializeField] private Game _game;
    [SerializeField] private RebornButton _rebornButton;
    [SerializeField] private float _timeRemaining = 3;
    [SerializeField] private TMP_Text _timeText;

    private bool _isRebornButtonClicked = false;
    private bool _timerIsRunning = false;

    private void OnValidate()
    {
        _timeRemaining = Mathf.Clamp(_timeRemaining, 0f, float.MaxValue);
    }

    private void OnEnable()
    {
        AddUpdate();
        _rebornButton.RebornButtonClicked += OnRebornButtonClicked;
    }

    private void OnDisable()
    {
        RemoveUpdate();
        _rebornButton.RebornButtonClicked -= OnRebornButtonClicked;
    }

    public override void OnTick()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.unscaledDeltaTime;
                DisplayTime(_timeRemaining);
            }
            else
            {
                _timeRemaining = 0;
                _timerIsRunning = false;
                _game.Reborn();
            }
        }
    }

    public void RunTimer()
    {
        if (_isRebornButtonClicked)
            _timerIsRunning = true;
    }

    private void OnRebornButtonClicked()
    {
        _isRebornButtonClicked = true;
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if(seconds != 0)
            _timeText.text = string.Format("{0}", seconds);
    }
}