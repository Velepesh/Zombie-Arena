using Plugins.Audio.Utils;
using UnityEngine;

public class BackgroundMusic : Audio
{
    [SerializeField] private AudioDataProperty _backgroundClip;

    public void Play()
    {
        SourceAudio.Play(_backgroundClip.Key);
    }
}