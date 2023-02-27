using UnityEngine;

public class TowerViewSetup : Setup
{
    [SerializeField] private Tower _tower;
    [SerializeField] private DamageableHealthView _view;

    private TowerViewPresenter _presenter;

    protected override void Awake()
    {
        _presenter = new TowerViewPresenter(_view, _tower);
    }

    protected override void OnEnable()
    {
        _presenter.Enable();
    }

    protected override void OnDisable()
    {
        _presenter.Disable();
    }
}