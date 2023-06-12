using UnityEngine;
using System;
using UnityEngine.Events;

public class TwinsCompositeRoot : Builder
{
    [SerializeField] private Twin _rightTwin;
    [SerializeField] private Twin _leftTwin;
    [SerializeField] private TwinsViewSetup _setup;

    public Twin RightTwin => _rightTwin;
    public Twin LeftTwin => _leftTwin;

    public event UnityAction<Twin> TwinDied;

    private void Awake()
    {
        _setup.enabled = false;
    }

    private void OnEnable()
    {
        _rightTwin.Died += OnTwinDied;
        _leftTwin.Died += OnTwinDied;
    }

    private void OnDisable()
    {
        _rightTwin.Died -= OnTwinDied;
        _leftTwin.Died -= OnTwinDied;
    }

    public override void Compose()
    {
        _setup.enabled = true;
    }

    public override void AddHealth(int value)
    {
        _rightTwin.Health.AddHealth(value);
        _leftTwin.Health.AddHealth(value);
    }

    public Twin GetAliveTwin(Twin currentTwin)
    {
        if (currentTwin.IsDied)
        {
            if (currentTwin == _leftTwin && _rightTwin.IsDied == false)
                return _rightTwin;
            else if(_rightTwin.IsDied == false)
                return _leftTwin;
        }

        throw new ArgumentNullException(nameof(currentTwin) + " must be died");
    }

    private void OnTwinDied(IDamageable damageable)
    {
        if (_rightTwin.IsDied && _leftTwin.IsDied)
            OnDied();
        else
            TwinDied?.Invoke(damageable as Twin);
    }
}