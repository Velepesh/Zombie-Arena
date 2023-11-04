using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIVirtualJoystick : MonoCache, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }

    [Header("Rect References")]
    public RectTransform containerRect;
    public RectTransform handleRect;

    [Header("Settings")]
    [SerializeField] private float _joystickRange = 120f;
    [SerializeField] private float _magnitudeMultiplier = 1f;

    [Header("Output")]
    [SerializeField] private Event _joystickOutputEvent;

    [SerializeField] private float _deadZone = 0;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Vector2 _scaleTouchInput;

    private Vector2 _touchInput, _prevDelta, _dragInput;
    private bool _isTouching = false;

    public event UnityAction PointerUp;

    public bool IsTouching => _isTouching;

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
        UpdateHandleRectPosition(Vector2.zero);
    }

    public override void OnTick()
    {
        _touchInput = _dragInput - _prevDelta;
        _prevDelta = _dragInput;
        _touchInput = Vector2.Scale(_touchInput, _scaleTouchInput);

        OutputPointerEventValue(_touchInput);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
         _prevDelta = _dragInput = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);

        position = ApplySizeDelta(position);
        Vector2 clampedPosition = ClampValuesToMagnitude(position);

        if (clampedPosition.magnitude > _deadZone)
            UpdateHandleRectPosition(clampedPosition * _joystickRange);
        else
            UpdateHandleRectPosition(Vector2.zero);

        _dragInput = eventData.position;
        _isTouching = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _touchInput = Vector2.zero;
       
        UpdateHandleRectPosition(Vector2.zero);

        _isTouching = false;
        PointerUp?.Invoke();
    }


    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        _joystickOutputEvent.Invoke(pointerPosition);
    }

    private void UpdateHandleRectPosition(Vector2 newPosition)
    {
        handleRect.anchoredPosition = newPosition;
    }

    Vector2 ApplySizeDelta(Vector2 position)
    {
        float x = (position.x/containerRect.sizeDelta.x) * 2.5f;
        float y = (position.y/containerRect.sizeDelta.y) * 2.5f;
        return new Vector2(x, y);
    }

    Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        return Vector2.ClampMagnitude(position, 1);
    }
}