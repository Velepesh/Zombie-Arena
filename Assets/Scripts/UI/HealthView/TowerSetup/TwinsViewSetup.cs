using UnityEngine;

public class TwinsViewSetup : Setup
{
    [SerializeField] private TwinsCompositeRoot _twinsCompositeRoot;
    [SerializeField] private DamageableHealthView _rightView;
    [SerializeField] private DamageableHealthView _leftView;

    private TwinViewPresenter _rightPresenter;
    private TwinViewPresenter _leftPresenter;

    protected override void Awake()
    {
        _rightPresenter = new TwinViewPresenter(_rightView, _twinsCompositeRoot.RightTwin);
        _leftPresenter = new TwinViewPresenter(_leftView, _twinsCompositeRoot.LeftTwin);
    }

    protected override void OnEnable()
    {
        _rightPresenter.Enable();
        _leftPresenter.Enable();
    }

    protected override void OnDisable()
    {
        _rightPresenter.Disable();
        _leftPresenter.Disable();
    }
}