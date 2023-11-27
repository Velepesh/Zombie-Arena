using UnityEngine;

public class DamagePanel : MonoBehaviour
{
    [SerializeField] private HudDamageScreen _damageScreen;
    [SerializeField] private bl_IndicatorManager _indicator;

    public void Init(Player player)
    {
        _indicator.Init(player);
    }

    public void OnAttacked(Zombie zombie)
    {
        _indicator.SetIndicator(zombie);
    }

    public void OnHealthChanged(int startHealth, int health)
    {
        _damageScreen.UpdateDamageScreen(startHealth, health);
    }
}
