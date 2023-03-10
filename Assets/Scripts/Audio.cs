using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour
{
    protected AudioSource AudioSource;

    protected virtual void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    protected virtual void PlayOneShot(AudioClip clip)
    {
        AudioSource.PlayOneShot(clip);
    }
}