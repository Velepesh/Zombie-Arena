using UnityEngine;
using UnityEngine.Events;
using YG;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private LangYGAdditionalText _levelText;

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
        SetValue(Level);
        LevelIncreased?.Invoke(Level);
    }

    private void SetValue(int value)
    {
        _levelText.additionalText = value.ToString();
    }
}