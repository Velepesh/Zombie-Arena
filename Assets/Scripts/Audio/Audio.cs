using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] protected AudioSource AudioSource;

    protected virtual void PlayOneShot(AudioClip clip)
    {
        AudioSource.PlayOneShot(clip);
    }
}