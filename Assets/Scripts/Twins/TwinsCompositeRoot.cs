using UnityEngine;

public class TwinsCompositeRoot : CompositeRoot
{
    [SerializeField] private Twins _twins;
    [SerializeField] private TwinsSetup _setup;

    public Twins Twins => _twins;

    public void Init()
    {
        _setup.Init(_twins);
    }

    public override void Compose()
    {
        _setup.enabled = true;
    }
}