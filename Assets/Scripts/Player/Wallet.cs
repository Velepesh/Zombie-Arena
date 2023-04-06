using System;

[Serializable]
public class Wallet
{
    public int Money { get; private set; }
    public bool HaveMoney => Money > 0;

    public event Action MoneyChanged;

    public Wallet(int money)
    {
        if (money < 0)
            throw new ArgumentOutOfRangeException(nameof(money));

        Money = money;
    }

    public void AddMoney(int value)
    {
        Money += value;

        MoneyChanged?.Invoke();
    }

    public void RemoveMoney(int value)
    {
        if (Money - value < 0)
        {
            Money = 0;
            return;
        }

        Money -= value;

        MoneyChanged?.Invoke();
    }
}
