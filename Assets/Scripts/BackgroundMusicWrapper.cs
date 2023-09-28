using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicWrapper : MonoBehaviour
{
    [SerializeField] private BackgroundMusic _backgroundMusic;

    public void DestroyOnly(BackgroundMusic backgroundMusic)
    {
        Destroy(backgroundMusic.gameObject);
    }
}
