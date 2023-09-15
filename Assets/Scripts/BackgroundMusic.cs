using UnityEngine;
using YG;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private bool _isExist;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;

        DisableMusic();
    }

    private void Load()
    {
        _isExist = YandexGame.savesData.IsExistBackgroundMusic;

        if (_isExist)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        EnableMusic();
    }

    private void Save()
    {
        if (_isExist == YandexGame.savesData.IsExistBackgroundMusic)
            return;

        YandexGame.savesData.IsExistBackgroundMusic = _isExist;
        YandexGame.SaveProgress();
    }

    private void EnableMusic()
    {
        _audioSource.ignoreListenerPause = true;
        _audioSource.Play();
        _isExist = true;
        Save();
    }

    private void DisableMusic()
    {
        _isExist = false;
        Save();
    }
}