using UnityEngine;

public class WalletSetup : MonoBehaviour
{
    [SerializeField] private WalletView _view;

    private WalletPresenter _presenter;
    private Wallet _model;

    private void Awake()
    {
        _model = new Wallet();
        _presenter = new WalletPresenter(_view, _model);
    }

    private void OnEnable()
    {
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}