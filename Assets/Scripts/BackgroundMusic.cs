using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    private AudioSource _audioSource;

    public event UnityAction Inited;
    public event UnityAction Disabled;


    public void Init(bool isExist)
    {
        if (isExist)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.ignoreListenerPause = true;
        _audioSource.Play();

        Inited?.Invoke();
    }

    private void OnDisable()
    {
        Disabled?.Invoke();
    }
}