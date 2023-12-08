using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelsHolder : MonoBehaviour
{
    [SerializeField] private List<Level> _classicLevels;   
    [SerializeField] private InfiniteLevel _infiniteLevel;   
    [SerializeField] private int _maxCircleClassicLevelIndex;
    [SerializeField] private int _minCircleClassicLevel;

    public InfiniteLevel InfiniteWavesLevel => _infiniteLevel;

    private void OnValidate()
    {
        _minCircleClassicLevel = Math.Clamp(_minCircleClassicLevel, 0, _classicLevels.Count - 1);
        _maxCircleClassicLevelIndex = Math.Clamp(_maxCircleClassicLevelIndex, _minCircleClassicLevel, _classicLevels.Count - 1);
    }

    public Level GetClassicLevel(int currentLevel)
    {
        int levelIndex = currentLevel - 1;

        if (levelIndex > _maxCircleClassicLevelIndex)
            levelIndex = _minCircleClassicLevel;

        return _classicLevels[levelIndex];
    }
}