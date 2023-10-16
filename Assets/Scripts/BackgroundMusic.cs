using Plugins.Audio.Core;
using Plugins.Audio.Utils;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private SourceAudio _sourceAudio;
    [SerializeField] private AudioDataProperty _clip;

    private void Start()
    {
        RunMusic();
    }

    private void RunMusic()
    {
        _sourceAudio.Play(_clip.Key);
    }

    //private void OnApplicationFocus(bool hasFocus)
    //{
    //    Silence(!hasFocus);
    //}

    //private void OnApplicationPause(bool isPaused)
    //{
    //    Silence(isPaused);
    //}

    private void Silence(bool silence)
    {
        AudioListener.pause = silence;
        AudioListener.volume = silence ? 0 : 1;
    }
}