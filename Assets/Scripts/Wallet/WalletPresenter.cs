public class WalletPresenter
{
    private WalletView _view;
    private Wallet _model;
    private WalletSaver _saver;

    public WalletPresenter(WalletView view, Wallet model, WalletSaver saver)
    {
        _view = view;
        _model = model;
        _saver = saver;
    }

    public void Enable()
    {
        _model.MoneyChanged += OnMoneyChanged;

        _view.SetWalletValue(_model.Money);
    }

    public void Disable()
    {
        _model.MoneyChanged -= OnMoneyChanged;
    }
     
    private void OnMoneyChanged(int value)
    {
        _view.SetWalletValue(value);
        _saver.OnMoneyChanged(value);
    }
}