using UnityEngine;
using UnityEngine.UI;
using YG;

public class HighscoreView : MonoBehaviour
{
    [SerializeField] private Image _medalImage;
    [SerializeField] private Sprite _goldMedalIcon;
    [SerializeField] private Sprite _silverMedalIcon;
    [SerializeField] private Sprite _bronzeMedalIcon;
    [SerializeField] private Sprite _unrankedIcon;
    [SerializeField] private LangYGAdditionalText _highscoreText;

    public void ShowRecorded(int score)
    {
        _highscoreText.additionalText = score.ToString();
    }

    public void SetMedalIcon(Highscore highscore)
    {
        int score = highscore.Value;
        Sprite currentSprite = null;

        if (score >= highscore.GoldScore)
            currentSprite = _goldMedalIcon;
        else if(score >= highscore.SilverScore)
            currentSprite = _silverMedalIcon;
        else if (score >= highscore.BronzeScore)
            currentSprite = _bronzeMedalIcon;
        else 
            currentSprite = _unrankedIcon;

        _medalImage.sprite = currentSprite;
    }
}
