using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletSaver : MonoBehaviour, ISaver
{
    [SerializeField] private WalletSetup _walletSetup;

    readonly string _savePath = "/wallet.json";

    private IDataService _dataService;
    private WalletData _data;

    private void Awake()
    {
        _dataService = new JsonDataService();
        LoadData();
    }

    private void OnEnable()
    {
        _walletSetup.Wallet.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable()
    {
        _walletSetup.Wallet.MoneyChanged -= OnMoneyChanged;
    }

    public void SaveData<WalletData>(string path, WalletData data)
    {
        _dataService.SaveData(path, data);
    }

    public void LoadData()
    {
        _data = _dataService.LoadData<WalletData>(_savePath);

        if (_data == null)
            _data = new WalletData();

        _walletSetup.Init(_data.Money);
    }

    private void OnMoneyChanged(int money)
    {
        _data.Money = money;
        SaveData(_savePath, _data);
    }
}
