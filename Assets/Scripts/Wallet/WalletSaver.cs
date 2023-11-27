using YG;
public class WalletSaver
{
    public WalletSaver()
    {
        Load();
    }

    public int Money { get; private set; }

    public void OnMoneyChanged(int money)
    {
        Money = money;
        Save();
    }

    private void Load()
    {
        Money = YandexGame.savesData.Money;
    }

    private void Save()
    {
        if (Money == YandexGame.savesData.Money)
            return;

        YandexGame.savesData.Money = Money;
        YandexGame.SaveProgress();
    }
}