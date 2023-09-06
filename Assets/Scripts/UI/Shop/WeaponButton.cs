using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class WeaponButton : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _boughtColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _choosedColor;

    private Image _image;
    private Button _button;

    public event UnityAction Clicked;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void SetBoughtColor()
    {
        SetImage(_boughtColor);
    }

    public void SetDefaultColor()
    {
        SetImage(_defaultColor);
    }

    public void SetChoosedColor()
    {
        SetImage(_choosedColor);
    }

    public void SetSelectedColor()
    {
        SetImage(_selectedColor);
    }

    private void OnButtonClick()
    {
        SetSelectedColor();
        Clicked?.Invoke();
    }

    private void SetImage(Color color)
    {
        _image.color = color;
    }
}