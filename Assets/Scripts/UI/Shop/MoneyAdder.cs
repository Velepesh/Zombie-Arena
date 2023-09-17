using TMPro;
using UnityEngine;
using YG;

public class MoneyAdder : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private int _money;
    [SerializeField] private WalletSetup _walletSetup;
    [SerializeField] private TMP_Text _moneyText;

    readonly private string _plus = "+";
    readonly private string _dollar = "$";

    private void OnValidate()
    {
        _money = Mathf.Clamp(_money, 0, int.MaxValue);
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    private void Start()
    {
        SetText(_money);
    }

    private void Rewarded(int id)
    {
        if (id == _adID)
            AddMoney();
    }

    private void AddMoney()
    {
        _walletSetup.Wallet.AddMoney(_money);
        SetText(_money);
    }

    private void SetText(int value)
    {
        _moneyText.text = $"{_plus}{value.ToString()}{_dollar}";
    }
}