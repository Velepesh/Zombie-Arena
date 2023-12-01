using UnityEngine;
using YG;

public class HighscoreView : MonoBehaviour
{
    [SerializeField] private LangYGAdditionalText _highscoreText;

    public void ShowRecorded(int value)
    {
        _highscoreText.additionalText = value.ToString();
    }
}
