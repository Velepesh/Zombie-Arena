using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityScreen = UnityEngine.Screen;

public class TouchZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UIVirtualJoystick _fireJoystick;
    [SerializeField] [Range(0f, 2f)] private float _scaleVector2;
    [HideInInspector]
    public Vector2 touchDist;
    [HideInInspector]
    public Vector2 pointerOld;
    [HideInInspector]
    protected int pointerId;
    [HideInInspector]
    public bool pressed;

    [Header("Output")]
    public UnityEvent<Vector2> joystickOutputEvent;
    void Update()
    {
        if (_fireJoystick.IsTouching)
            return;

        if (pressed)
        {
            if (Input.touchCount > 1)
            {
                Touch touch = Input.touches[pointerId];
                if (touch.position.x > UnityScreen.width / 2f)
                {
                    touchDist = touch.position - pointerOld;
                    pointerOld = touch.position;
                }
                else
                {
                    touchDist = Vector2.zero;
                }
            }
            else
            {
                Touch touch = Input.GetTouch(0);

                if (touch.position.x > UnityScreen.width / 2f)
                {
                    touchDist = touch.position - pointerOld;
                    touchDist = Vector2.Scale(touchDist, new Vector2(_scaleVector2, _scaleVector2));
                    pointerOld = touch.position;
                }
                else
                {
                    touchDist = Vector2.zero;
                }
            }
        }
        else
        {
            touchDist = Vector2.zero;
        }
     
        OutputPointerEventValue(touchDist);
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent?.Invoke(pointerPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        OutputPointerEventValue(Vector2.zero);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.position.x > UnityScreen.width / 2f)
        {
            pressed = true;
            pointerId = eventData.pointerId;
            pointerOld = eventData.position;
        }
    }
}