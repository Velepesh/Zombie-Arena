using UnityEngine.Audio;

public class Volume
{
    private AudioMixer _mixer;
    private float _sfx;
    private float _music;

    readonly string SFX = "SFX";
    readonly string MUSIC = "Music";

    public Volume(AudioMixer mixer, float sfx, float music)
    {
        _mixer = mixer;
        Load(music, sfx);
    }

    public float Sfx => _sfx;
    public float Music => _music;

    private void Load(float music, float sfx)
    {
        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }

    public void SetMusicVolume(float value) 
    {
        _music = value;
        _mixer.SetFloat(MUSIC, _music);
    }

    public void SetSFXVolume(float value)
    {
        _sfx = value;
        _mixer.SetFloat(SFX, _sfx);
    }
}