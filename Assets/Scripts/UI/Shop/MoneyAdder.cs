using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyAdder : MonoBehaviour
{
    [SerializeField] private int _money;
    [SerializeField] private WalletSetup _walletSetup;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _moneyText;

    readonly private string _plus = "+";
    readonly private string _dollar = "$";

    private void OnValidate()
    {
        _money = Mathf.Clamp(_money, 0, int.MaxValue);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void Start()
    {
        SetText(_money);
    }

    private void SetText(int value)
    {
        _moneyText.text = $"{_plus}{value.ToString()}{_dollar}";
    }

    private void OnButtonClick()
    {
        _walletSetup.Wallet.AddMoney(_money);
    }
}