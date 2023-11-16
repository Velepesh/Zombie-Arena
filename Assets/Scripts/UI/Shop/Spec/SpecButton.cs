using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpecButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private CanvasGroup _canvasGroup;

    public event UnityAction Clicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void EnableButton()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void DisableButton()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void OnButtonClick()
    {
        Clicked?.Invoke();
    }
}