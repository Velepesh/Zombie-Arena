public class WalletPresenter
{
    private WalletView _view;
    private Wallet _model;

    public WalletPresenter(WalletView view, Wallet model)
    {
        _view = view;
        _model = model;
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
    }
}