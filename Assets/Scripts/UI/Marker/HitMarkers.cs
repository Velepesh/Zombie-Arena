using UnityEngine;

public class HitMarkers : MonoBehaviour
{
    [SerializeField] private Marker _killMark;
    [SerializeField] private Marker _headshotMark;
    [SerializeField] private Marker _bodyShotMark;
    [SerializeField] private Marker _killHeadMark;
    [SerializeField] private Marker _skullImage;
    [SerializeField] private float _lineDuration;
    [SerializeField] private float _skullDuration;

    private ZombiesSpawner _spawner;

    private void OnValidate()
    {
        _lineDuration = Mathf.Clamp(_lineDuration, 0f, float.MaxValue);
        _skullDuration = Mathf.Clamp(_skullDuration, 0f, float.MaxValue);
    }

    public void Init(ZombiesSpawner spawner)
    {
        _spawner = spawner;

        _spawner.ZombieDied += OnZombieDied;
        _spawner.HeadshotReceived += OnHeadshotReceived;
        _spawner.BodyshotReceived += OnBodyshotReceived;
    }

    private void OnDisable()
    {
        if (_spawner != null)
        {
            _spawner.ZombieDied -= OnZombieDied;
            _spawner.HeadshotReceived -= OnHeadshotReceived;
            _spawner.BodyshotReceived -= OnBodyshotReceived;
        }
    }

    private void OnZombieDied(Zombie zombie)
    {
        if (zombie.IsHeadKill)
            ShowSkull();
        else
            ShowKillMark();
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
        _killHeadMark.Show(_skullDuration);
        _skullImage.Show(_skullDuration);
    }
}