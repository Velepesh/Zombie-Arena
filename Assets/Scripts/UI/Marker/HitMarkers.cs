using UnityEngine;

public class HitMarkers : MonoBehaviour
{
    [SerializeField] private ZombieSpawner _spawner;
    [SerializeField] private Marker _killMark;
    [SerializeField] private Marker _headshotMark;
    [SerializeField] private Marker _bodyShotMark;
    [SerializeField] private Marker _skullImage;
    [SerializeField] private float _lineDuration;
    [SerializeField] private float _skullDuration;

    private void OnValidate()
    {
        _lineDuration = Mathf.Clamp(_lineDuration, 0f, float.MaxValue);
        _skullDuration = Mathf.Clamp(_skullDuration, 0f, float.MaxValue);
    }

    private void OnEnable()
    {
        _spawner.ZombieDied += OnZombieDied;
        _spawner.HeadshotReceived += OnHeadshotReceived;
        _spawner.BodyshotReceived += OnBodyshotReceived;
    }

    private void OnDisable()
    {
        _spawner.ZombieDied -= OnZombieDied;
        _spawner.HeadshotReceived -= OnHeadshotReceived;
        _spawner.BodyshotReceived -= OnBodyshotReceived;
    }

    private void OnZombieDied(Zombie zombie)
    {
        ShowKillMark();

        if (zombie.LastDamageHandlerType == DamageHandlerType.Head)
            ShowSkull();
    }

    private void OnHeadshotReceived()
    {
        ShowHeadMark();
    }

    private void OnBodyshotReceived()
    {
        ShowBodyMark();
    }

    private void ShowKillMark()
    {
        _killMark.Show(_lineDuration);
    }

    private void ShowHeadMark()
    {
        _headshotMark.Show(_lineDuration);
    }

    private void ShowBodyMark()
    {
        _bodyShotMark.Show(_lineDuration);
    }

    private void ShowSkull()
    {
        _skullImage.Show(_skullDuration);
    }
}