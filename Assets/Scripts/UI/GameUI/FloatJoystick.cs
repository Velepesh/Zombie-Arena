using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using UnityScreen = UnityEngine.Screen;

public class FloatJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;

    [Header("Output")]
    public UnityEvent<Vector2> joystickOutputEvent;

    private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;
    private bool _isTouching;

    private Vector2 input = Vector2.zero;

    public float InputY => input.y;

    public event UnityAction PointerUp;

    protected virtual void Start()
    {
        baseRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(false);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.position.x < UnityScreen.width / 2)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            OnDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.x < UnityScreen.width / 2)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            if(_isTouching == false)
                background.gameObject.SetActive(true);

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized);
            handle.anchoredPosition = input * radius * handleRange;
            OutputPointerEventValue(input);
            _isTouching = true;
        }
        else
        {
            _isTouching = false;
        }


        if (_isTouching == false)
            OnPointerUp(eventData);
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent?.Invoke(pointerPosition);
    }

    private void FormatInput()
    {
        if (axisOptions == AxisOptions.Horizontal)
            input = new Vector2(input.x, 0f);
        else if (axisOptions == AxisOptions.Vertical)
            input = new Vector2(0f, input.y);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        input = Vector2.zero;
        HandleInput(input.magnitude, input.normalized);
        OutputPointerEventValue(input);
        handle.anchoredPosition = Vector2.zero;
        PointerUp?.Invoke();
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }
}
