using UnityEngine;
using UnityEngine.UI;
using YG;
using UnityEngine.Events;

public class HealthAdder : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private string _label;
    [SerializeField] private Builder _builderCompositeRoot;
    [SerializeField] private int _addedHealth = 25;
    [SerializeField] private int _price;
    [SerializeField] private Button _buyButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasFade _canvasFade;

    private Health _health;

    public int Price => _price;
    public string Label => _label;

    public event UnityAction<int> HealthChanged;
    public event UnityAction<HealthAdder, int> BuyHealthButtonClicked;

    private void OnValidate()
    {
        _addedHealth = Mathf.Clamp(_addedHealth, 0, int.MaxValue);
        _price = Mathf.Clamp(_price, 0, int.MaxValue);
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
        _builderCompositeRoot.HealthLoaded += OnHealthLoaded;
        _buyButton.onClick.AddListener(OnBuyHealthButtonClick);
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
        _buyButton.onClick.RemoveListener(OnBuyHealthButtonClick);

        if(_health != null)
            _health.HealthChanged -= OnHealthChanged;
    }

    public void AddHealth()
    {
        _builderCompositeRoot.AddHealth(_addedHealth);
        MetricaSender.Reward("health_adder");
    }

    public void HideHealthAdderPanel()
    {
        if(_canvasGroup.alpha != 0)
            _canvasFade.Hide();
    }

    private void OnHealthLoaded(Health health)
    {
        _health = health;
        _health.HealthChanged += OnHealthChanged;
        OnHealthChanged(_health.Value);
    }

    private void OnHealthChanged(int value)
    {
        HealthChanged?.Invoke(value);
    }

    private void Rewarded(int id)
    {
        if (id == _adID)
            AddHealth();
    }

    private void OnBuyHealthButtonClick()
    {
        BuyHealthButtonClicked?.Invoke(this, _price);
    }
}