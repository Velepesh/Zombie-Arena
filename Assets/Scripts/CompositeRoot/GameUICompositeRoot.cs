using UnityEngine;

public class GameUICompositeRoot : CompositeRoot
{
    [SerializeField] private Canvas _gameCanvas;
    [SerializeField] private Canvas _glowCanvas;

    private void Awake()
    {
        DisableCanvas(_gameCanvas);
        DisableCanvas(_glowCanvas);
    }

    public override void Compose()
    {
        EnableCanvas(_gameCanvas);
        EnableCanvas(_glowCanvas);
    }

    private void DisableCanvas(Canvas canvas)
    {
        canvas.gameObject.SetActive(false);
    }

    private void EnableCanvas(Canvas canvas)
    {
        canvas.gameObject.SetActive(true);
    }
}
