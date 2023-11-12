using UnityEngine;
using YG;

public class ConsumePurchase : MonoBehaviour
{
    private static bool _consume;
    
    private void OnEnable()
    {
        YandexGame.GetPaymentsEvent += ConsumePurchases;
        YandexGame.GetDataEvent += ConsumePurchases;
    }

    private void OnDisable()
    {
        YandexGame.GetPaymentsEvent -= ConsumePurchases;
        YandexGame.GetDataEvent -= ConsumePurchases;
    }

    private void Start()
    {
        if (YandexGame.SDKEnabled)
            ConsumePurchases();
    }

    private void ConsumePurchases()
    {
        if (_consume == false)
        {
            _consume = true;
            YandexGame.ConsumePurchases();
        }
    }
}