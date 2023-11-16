using UnityEngine;

public class MusicCompositeRoot : CompositeRoot
{
    [SerializeField] private BackgroundMusic _backgroundMusic;

    public override void Compose()
    {
        _backgroundMusic.Play();
    }
}