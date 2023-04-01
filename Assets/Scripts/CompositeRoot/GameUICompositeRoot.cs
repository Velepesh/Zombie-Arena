using UnityEngine;

public class GameUICompositeRoot : CompositeRoot
{
    [SerializeField] private Canvas _gameCanvas;

    private void Awake()
    {
        _gameCanvas.gameObject.SetActive(false);
    }

    public override void Compose()
    {
        _gameCanvas.gameObject.SetActive(true);
    }
}
