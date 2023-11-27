using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityScreen = UnityEngine.Screen;

public class FloatJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;

    [Header("Output")]
    public UnityEvent<Vector2> joystickOutputEvent;

    private RectTransform _baseRect = null;

    private Canvas _canvas;
    private Camera _cam;

    private Vector2 input = Vector2.zero;

    public float InputY => input.y;

    public event UnityAction PointerUp;

    protected virtual void Start()
    {
        _baseRect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
        ObjectEnabler.Disable(background.gameObject);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.position.x < UnityScreen.width / 2)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            ObjectEnabler.Enable(background.gameObject);
            OnDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cam = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            _cam = _canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * _canvas.scaleFactor);
        HandleInput(input.magnitude, input.normalized);
        handle.anchoredPosition = input * radius * handleRange;
        OutputPointerEventValue(input);
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
        {
            input = Vector2.zero;
        }
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent?.Invoke(pointerPosition);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ObjectEnabler.Disable(background.gameObject);
        input = Vector2.zero;
        HandleInput(input.magnitude, input.normalized);
        OutputPointerEventValue(input);
        handle.anchoredPosition = Vector2.zero;
        PointerUp?.Invoke();
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _cam, out localPoint))
        {
            Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
            return localPoint - (background.anchorMax * _baseRect.sizeDelta) + pivotOffset;
        }

        return localPoint;
    }
}
