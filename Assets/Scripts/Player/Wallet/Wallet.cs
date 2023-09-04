using System;
using UnityEngine;

[Serializable]
public class Wallet
{
    readonly private string BALANCE = "Balance";

    private int _money => PlayerPrefs.GetInt(BALANCE, 0);

    public int Money { get; private set; }
    public bool HaveMoney => Money > 0;

    public delegate void MoneyChangedHandler(int value);
    public event MoneyChangedHandler MoneyChanged;

    public Wallet()
    {
        if (_money < 0)
            throw new ArgumentOutOfRangeException(nameof(_money));

        Money = _money;
    }

    public void AddMoney(int value)
    {
        Money += value;

        MoneyChanged?.Invoke(Money);
        SaveBalance();
    }

    public void RemoveMoney(int value)
    {
        if (Money - value < 0)
        {
            Money = 0;
            return;
        }

        Money -= value;

        MoneyChanged?.Invoke(Money);
        SaveBalance();
    }

    private void SaveBalance()
    {
        PlayerPrefs.SetInt(BALANCE, Money);
    }
}