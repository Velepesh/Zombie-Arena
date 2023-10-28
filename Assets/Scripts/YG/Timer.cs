using UnityEngine;

public class Timer : MonoCache
{
    private float _spentTime = 0f;
    private bool _isPlaying;

    public int SpentTime => (int)_spentTime;

    private void OnEnable()
    {
        AddUpdate();
    }

    private void OnDisable()
    {
        RemoveUpdate();
    }

    public override void OnTick()
    {
        if (_isPlaying == false)
            return;

        _spentTime += Time.deltaTime;
    }

    public void StartTimer()
    {
        _isPlaying = true;
    }

    public void StopTimer()
    {
        _isPlaying = false;
    }
}