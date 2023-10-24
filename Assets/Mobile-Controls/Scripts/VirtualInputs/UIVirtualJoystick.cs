using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIVirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }

    [Header("Rect References")]
    public RectTransform containerRect;
    public RectTransform handleRect;

    [Header("Settings")]
    public float joystickRange = 50f;
    public float magnitudeMultiplier = 1f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;

    [Header("Output")]
    public Event joystickOutputEvent;

    [SerializeField] private float _deadZone = 0;
    [SerializeField] private float _activeJoystickDistance;
    [SerializeField] private Canvas _canvas;

    private bool isTouching = false;

    public event UnityAction PointerUp;

    private float _inputY;

    public float InputY => _inputY;
    public bool IsTouching => isTouching;

    void Start()
    {
        _activeJoystickDistance = _activeJoystickDistance * _canvas.scaleFactor;
        SetupHandle();
    }

    private void SetupHandle()
    {
        if(handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Vector2.Distance(eventData.position, containerRect.position) < _activeJoystickDistance)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);

            position = ApplySizeDelta(position);

            Vector2 clampedPosition = ClampValuesToMagnitude(position);

            if (clampedPosition.magnitude > _deadZone)
            {
                Vector2 outputPosition = ApplyInversionFilter(position);

                OutputPointerEventValue(outputPosition * magnitudeMultiplier);

                if (handleRect)
                    UpdateHandleRectPosition(clampedPosition * joystickRange);
            }
            else
            {
                Vector2 outputPosition = Vector2.zero;
                OutputPointerEventValue(outputPosition);
                if (handleRect)
                    UpdateHandleRectPosition(outputPosition);
            }

            _inputY = clampedPosition.y;
            isTouching = true;
        }
        else
        {
            isTouching = false;
        }

        if (isTouching == false)
            OnPointerUp(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OutputPointerEventValue(Vector2.zero);

        if(handleRect)
             UpdateHandleRectPosition(Vector2.zero);

        isTouching = false;
        PointerUp?.Invoke();
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent.Invoke(pointerPosition);
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

    Vector2 ApplyInversionFilter(Vector2 position)
    {
        if(invertXOutputValue)
        {
            position.x = InvertValue(position.x);
        }

        if(invertYOutputValue)
        {
            position.y = InvertValue(position.y);
        }

        return position;
    }

    float InvertValue(float value)
    {
        return -value;
    }
    
}
/*
 public void OnDrag(PointerEventData eventData)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
            {
                Debug.Log(touch.position);

                Debug.Log(touch.position);
                if (Vector2.Distance(touch.position, containerRect.position) < _activeJoystickDistance)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);

                    position = ApplySizeDelta(position);

                    Vector2 clampedPosition = ClampValuesToMagnitude(position);

                    if (clampedPosition.magnitude > _deadZone)
                    {
                        Vector2 outputPosition = ApplyInversionFilter(position);

                        OutputPointerEventValue(outputPosition * magnitudeMultiplier);

                        if (handleRect)
                            UpdateHandleRectPosition(clampedPosition * joystickRange);
                    }
                    else
                    {
                        Vector2 outputPosition = Vector2.zero;
                        OutputPointerEventValue(outputPosition);
                        if (handleRect)
                            UpdateHandleRectPosition(outputPosition);
                    }

                    _inputY = clampedPosition.y;
                    isTouching = true;
                }
                else
                {
                    isTouching = false;
                }


                if (isTouching == false)
                    OnPointerUp(eventData);

                break;
            }
        }
    }

 */