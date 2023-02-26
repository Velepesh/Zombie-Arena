using UnityEngine;

public class TowerViewSetup : MonoBehaviour
{
    [SerializeField] private Tower _tower;
    [SerializeField] private DamageableHealthView _view;

    private TowerViewPresenter _presenter;

    private void Awake()
    {
        _presenter = new TowerViewPresenter(_view, _tower);
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
