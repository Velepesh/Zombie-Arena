using UnityEngine;

public class WalletSetup : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private WalletView _view;

    private WalletPresenter _presenter;
    private Wallet _model;

    public Wallet Wallet => _model;

    public void Init(int money)
    {
        _model = new Wallet(money);
        _presenter = new WalletPresenter(_view, _model, _game);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}