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
}