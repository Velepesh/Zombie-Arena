using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Game _game;

    private readonly string _levelName = "Уровень";
    
    public int Level { get; private set; }

    public event UnityAction<int> LevelIncreased;

    public void Init(int level)
    {
        Level = level;
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
        Level++;
        LevelIncreased?.Invoke(Level);
        SetValue(Level);
    }

    private void SetValue(int value)
    {
        _levelText.text = $"{_levelName} {value.ToString()}";
    }
}