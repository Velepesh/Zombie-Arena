using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIButtonsSound : MonoBehaviour
{
    [SerializeField] private AudioClip _buttonClickAudioClip;

    private AudioSource _audioSource;
    private Button[] _buttons;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _buttons = GetComponentsInChildren<Button>();
        _audioSource.ignoreListenerPause = true;
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
        _audioSource.PlayOneShot(_buttonClickAudioClip);
    }
}