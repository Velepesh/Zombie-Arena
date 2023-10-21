using UnityEngine;
using UnityEngine.UI;

public class IconButtonChanger : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _firstIcon;
    [SerializeField] private Sprite _secondIcon;
    [SerializeField] private Button _button;

    private bool _isFirstIcon = true;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void Start()
    {
        SetIcon(_firstIcon);
    }

    private void OnButtonClick() 
    {
        if (_isFirstIcon)
        {
            SetIcon(_secondIcon);
            _isFirstIcon = false;
        }
        else
        {
            SetIcon(_firstIcon);
            _isFirstIcon = true;
        }
    }

    private void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }
}