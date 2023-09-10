
public class WalletPresenter
{
    private WalletView _view;
    private Wallet _model;
    private Game _game;

    public WalletPresenter(WalletView view, Wallet model, Game game)
    {
        _view = view;
        _model = model;
        _game = game;
    }

    public void Enable()
    {
        _game.Won += OnMoneyAdded;
        _game.GameOver += OnMoneyAdded;
        _model.MoneyChanged += OnMoneyChanged;

        _view.SetWalletValue(_model.Money);
    }

    public void Disable()
    {
        _game.Won -= OnMoneyAdded;
        _game.GameOver -= OnMoneyAdded;
        _model.MoneyChanged -= OnMoneyChanged;
    }
     
    private void OnMoneyAdded()
    {
        _model.AddMoney(_game.Score.TotalScore);
    }

    private void OnMoneyChanged(int value)
    {
        _view.SetWalletValue(value);
    }
}