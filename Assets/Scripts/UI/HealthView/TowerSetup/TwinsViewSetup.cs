using UnityEngine;

public class TwinsViewSetup : Setup
{
    [SerializeField] private TwinsCompositeRoot _twinsCompositeRoot;
    [SerializeField] private DamageableHealthView _view;

    private TwinViewPresenter _presenter;

    protected override void Awake()
    {
        _presenter = new TwinViewPresenter(_view, _twinsCompositeRoot.Twins);
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