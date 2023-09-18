using UnityEngine;
using UnityEngine.UI;
using YG;
using UnityEngine.Events;

public class HealthAdder : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private Builder _builderCompositeRoot;
    [SerializeField] private int _health;
    [SerializeField] private int _price;
    [SerializeField] private Button _buyButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasFade _canvasFade;

    public event UnityAction<HealthAdder, int> BuyHealthButtonClicked;

    private void OnValidate()
    {
        _health = Mathf.Clamp(_health, 0, int.MaxValue);
        _price = Mathf.Clamp(_price, 0, int.MaxValue);
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
        _buyButton.onClick.AddListener(OnBuyHealthButtonClick);
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
        _buyButton.onClick.RemoveListener(OnBuyHealthButtonClick);
    }

    public void AddHealth()
    {
        _builderCompositeRoot.AddHealth(_health);
    }

    public void HideHealthAdderPanel()
    {
        if(_canvasGroup.alpha != 0)
            _canvasFade.Hide();
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