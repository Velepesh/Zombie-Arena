using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Game _game;

    private readonly string _levelName = "Уровень";
    private int _level;

    public event UnityAction<int> LevelIncreased;

    public void Init(int level)
    {
        _level = level;
        SetValue(level);
    }

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
        _level++;
        LevelIncreased?.Invoke(_level);
    }

    private void SetValue(int value)
    {
        _levelText.text = $"{_levelName} {value.ToString()}";
    }
}