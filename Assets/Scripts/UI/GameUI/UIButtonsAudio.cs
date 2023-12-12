using Plugins.Audio.Utils;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonsAudio : Audio
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioDataProperty _clip;

    private Button[] _buttons;

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();
    }

    private void OnEnable()
    {
        _audioSource.ignoreListenerPause = true;
        for (int i = 0; i < _buttons.Length; i++)
            _buttons[i].onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _buttons.Length; i++)
            _buttons[i].onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        SourceAudio.PlayOneShot(_clip.Key);
    }
}