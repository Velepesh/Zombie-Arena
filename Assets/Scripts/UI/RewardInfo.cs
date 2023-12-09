using TMPro;
using UnityEngine;
using YG;

public class RewardInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _doubleRewardText;
    [SerializeField] private LangYGAdditionalText _additionalRewardText;

    private readonly string _rightBracket = "]";
    private readonly string _dollar = "$";

    public void ShowInfo(int reward)
    {
        Debug.Log("ShowInfo");
        int doubleReward = reward * 2; 
        _doubleRewardText.text = $"{doubleReward} {_dollar}";
        _additionalRewardText.additionalText = $"{reward}{_dollar} {_rightBracket}";
    }
}