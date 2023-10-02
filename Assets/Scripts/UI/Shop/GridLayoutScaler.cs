using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutScaler : MonoCache
{
    [SerializeField] private Canvas _copyOfMainCanvas;
    [SerializeField] private Vector2 _baseSize = new Vector2(1920, 1080);
    
    private Vector2 _baseCellSize;
    private Vector2 _baseCellSpacing;
    private GridLayoutGroup _layoutGroup;

    private void OnEnable()
    {
        AddUpdate();
    }

    private void OnDisable()
    {
        RemoveUpdate();
    }

    private void Start()
    {
        _layoutGroup = GetComponent<GridLayoutGroup>();
        _baseCellSize = _layoutGroup.cellSize;
        _baseCellSpacing = _layoutGroup.spacing;
    }

    public override void OnTick()
    {
        float scaleFactor = _copyOfMainCanvas.scaleFactor;
        Vector2 screenSize = new Vector2(Screen.width / scaleFactor, Screen.height / scaleFactor);
        _layoutGroup.cellSize = (screenSize / _baseSize) * _baseCellSize;
        _layoutGroup.spacing = (screenSize / _baseSize) * _baseCellSpacing;
    }
}