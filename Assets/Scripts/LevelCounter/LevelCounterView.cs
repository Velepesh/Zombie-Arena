using UnityEngine;
using YG;

public class LevelCounterView : MonoBehaviour
{
    [SerializeField] private LangYGAdditionalText _levelText;

    public void OnLevelIncreased(int value)
    {
        _levelText.additionalText = value.ToString();
    }
}
