using UnityEngine;
using UnityEngine.UI;

public class HealthAdder : MonoBehaviour
{
    [SerializeField] private Builder _builderCompositeRoot;
    [SerializeField] private int _health;
    [SerializeField] private Button _button;

    private void OnValidate()
    {
        _health = Mathf.Clamp(_health, 0, int.MaxValue);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        _builderCompositeRoot.AddHealth(_health);
        _button.interactable = false;
    }
}