using UnityEngine;

public class WalletSetup : MonoBehaviour
{
    [SerializeField] private WalletView _view;

    private WalletSaver _saver;
    private WalletPresenter _presenter;
    private Wallet _model;

    public Wallet Wallet => _model;

    public void Init()
    {
        _saver = new WalletSaver();
        _model = new Wallet(_saver.Money);
        _presenter = new WalletPresenter(_view, _model, _saver);
        _presenter.Enable();
    }


    private void OnDisable()
    {
        _presenter.Disable();
    }
}