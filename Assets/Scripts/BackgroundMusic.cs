using UnityEngine;
//using YG;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private bool _isExist;
    private bool _isDestroying;

    private void Awake()
    {
        //if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        //YandexGame.GetDataEvent += Load;
    }

    private void OnDisable()
    {
        //YandexGame.GetDataEvent -= Load;

        //DisableMusic();
    }

    private void Load()
    {
        //_isExist = YandexGame.savesData.IsExistBackgroundMusic;

        //if (_isExist)
        //{
        //    _isDestroying = true;
        //    Destroy(gameObject);
        //    return;
        //}

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        EnableMusic();
    }

    private void Save()
    {
        //if (_isExist == YandexGame.savesData.IsExistBackgroundMusic)
        //    return;

        //YandexGame.savesData.IsExistBackgroundMusic = _isExist;
        //YandexGame.SaveProgress();
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
        //if (_isDestroying)
        //    return;

        _isExist = false;
        Save();
    }
}