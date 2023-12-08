using UnityEngine;
using YG;

public class HealthGoodsView : MonoBehaviour
{
    [SerializeField] private HealthGoods _healthGoods;
    [SerializeField] private LangYGAdditionalText _valueText;
    
    private void OnEnable()
    {
        _healthGoods.Inited += OnInited;
    }

    private void OnDisable()
    {
        _healthGoods.Inited -= OnInited;
    }

    private void OnInited(int health)
    {
        _valueText.additionalText = health.ToString();
    }
}