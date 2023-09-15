using UnityEngine;
using YG;

public class WalletSaver : MonoBehaviour
{
    [SerializeField] private WalletSetup _walletSetup;

    private int _money;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
        _walletSetup.Wallet.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
        _walletSetup.Wallet.MoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(int money)
    {
        _money = money;
        Save();
    }

    private void Load()
    {
        _money = YandexGame.savesData.Money;

        _walletSetup.Wallet.AddMoney(_money);
    }

    private void Save()
    {
        if (_money == YandexGame.savesData.Money)
            return;

        YandexGame.savesData.Money = _money;
        YandexGame.SaveProgress();
    }
}