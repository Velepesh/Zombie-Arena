using System;

public class Wallet
{
    public int Money { get; private set; }

    public delegate void MoneyChangedHandler(int value);
    public event MoneyChangedHandler MoneyChanged;

    public void AddMoney(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Money += value;

        MoneyChanged?.Invoke(Money);
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
    }
}