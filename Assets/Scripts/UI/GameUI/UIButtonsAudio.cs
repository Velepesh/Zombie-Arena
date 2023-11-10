using UnityEngine;
using UnityEngine.UI;

public class UIButtonsAudio : Audio
{
    [SerializeField] private AudioClip _buttonClickAudioClip;

    private Button[] _buttons;

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();
        AudioSource.ignoreListenerPause = true;
    }

    private void OnEnable()
    {
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
        PlayOneShot(_buttonClickAudioClip);
    }
}