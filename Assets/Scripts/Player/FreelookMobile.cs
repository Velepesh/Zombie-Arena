using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityScreen = UnityEngine.Screen;

public class FreelookMobile : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IEndDragHandler
{
    Image imgCameraZone;

    [Header("Output")]
    public UnityEvent<Vector2> joystickOutputEvent;

    private void Start()
    {
        imgCameraZone = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.x > UnityScreen.width / 2f)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imgCameraZone.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 positionOut))
            {
                OutputPointerEventValue(eventData.delta);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OutputPointerEventValue(Vector2.zero);
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent?.Invoke(pointerPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        OutputPointerEventValue(Vector2.zero);
    }
}