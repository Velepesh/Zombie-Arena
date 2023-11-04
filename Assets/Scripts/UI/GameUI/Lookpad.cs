using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityScreen = UnityEngine.Screen;

[RequireComponent(typeof(CanvasGroup))]
public class Lookpad : MonoCache, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UIVirtualJoystick _fireJoystick;
    [SerializeField] private Vector2 _scaleTouchInput;

    private Vector2 _touchInput, _prevDelta, _dragInput;
    private CanvasGroup _canvasGroup;

    [Header("Output")]
    public UnityEvent<Vector2> joystickOutputEvent;

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
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    public override void OnTick()
    {
        if (_fireJoystick.IsTouching)
            return;

        _touchInput = _dragInput - _prevDelta;
        _prevDelta = _dragInput;

        _touchInput = Vector2.Scale(_touchInput, _scaleTouchInput);
        OutputPointerEventValue(_touchInput);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.position.x > UnityScreen.width / 2f)
            _prevDelta = _dragInput = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragInput = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _touchInput = Vector2.zero;
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent?.Invoke(pointerPosition);
    }
}