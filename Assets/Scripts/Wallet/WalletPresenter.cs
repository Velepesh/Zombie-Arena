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
        _game.GameOver += OnGameOver;//«¿Ã≈Õ»“‹ Õ¿ WON
        _model.MoneyChanged += OnMoneyChanged;

        _view.SetWalletValue(_model.Money);
    }

    public void Disable()
    {
        _game.GameOver -= OnGameOver;
        _model.MoneyChanged -= OnMoneyChanged;
    }
     
    private void OnGameOver()
    {
        _model.AddMoney(_game.Score.TotalScore);
    }

    private void OnMoneyChanged(int value)
    {
        _view.SetWalletValue(value);
    }
}