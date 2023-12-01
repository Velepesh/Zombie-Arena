using TMPro;
using UnityEngine;
using YG;

public class RewardInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _doubleRewardText;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private LangYGAdditionalText _additionalRewardText;

    private readonly string _rightBracket = "]";
    private readonly string _dollar = "$";

    private string _startRewardText;
    private bool _isStartTexSetted;

    public void ShowInfo(int reward)
    {
        TrySetStartRewardText();

        int doubleReward = reward * 2; 
        _doubleRewardText.text = $"{doubleReward} {_dollar}";
        _rewardText.text = _startRewardText;
        _additionalRewardText.additionalText = $"{reward}{_dollar} {_rightBracket}";
    }

    private void TrySetStartRewardText()
    {
        if (_isStartTexSetted == false)
        {
            _startRewardText = _rewardText.text;
            _isStartTexSetted = true;
        }
    }
}