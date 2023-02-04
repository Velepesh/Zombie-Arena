using UnityEngine;

public class HitMarkers : MonoBehaviour
{
    [SerializeField] private ZombieSpawner _spawner;
    [SerializeField] private Marker _headshotMark;
    [SerializeField] private Marker _bodyShotMark;
    [SerializeField] private float _duration;

    private void OnValidate()
    {
        _duration = Mathf.Clamp(_duration, 0f, float.MaxValue);
    }

    private void OnEnable()
    {
        _spawner.HeadshotReceived += OnHeadshotReceived;
        _spawner.BodyshotReceived += OnBodyshotReceived;
    }

    private void OnDisable()
    {
        _spawner.HeadshotReceived -= OnHeadshotReceived;
        _spawner.BodyshotReceived -= OnBodyshotReceived;
    }

    private void OnHeadshotReceived()
    {
        ShowHeadMark();
    }

    private void OnBodyshotReceived()
    {
        ShowBodyMark();
    }

    private void ShowHeadMark()
    {
        _headshotMark.Show(_duration);
    }

    private void ShowBodyMark()
    {
        _bodyShotMark.Show(_duration);
    }
}