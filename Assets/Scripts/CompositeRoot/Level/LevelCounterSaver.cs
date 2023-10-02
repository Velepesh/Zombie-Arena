using UnityEngine;
using YG;

public class LevelCounterSaver : MonoBehaviour
{
    [SerializeField] private LevelCounter _levelCounter;

    private int _level;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
        _levelCounter.LevelIncreased += OnLevelIncreased;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
        _levelCounter.LevelIncreased -= OnLevelIncreased;
    }

    private void Load()
    {
        _level = YandexGame.savesData.Level;

        _levelCounter.Init(_level);
    }

    private void Save()
    {
        if (_level == YandexGame.savesData.Level)
            return;

        YandexGame.savesData.Level = _level;
         YandexGame.SaveProgress();
    }

    private void OnLevelIncreased(int level)
    {
        if (_level == level)
            return;

        _level = level;
        Save();
    }
}