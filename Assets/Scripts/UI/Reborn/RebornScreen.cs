using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebornScreen : MonoBehaviour
{
    [SerializeField] private Button _clickToContinueButton;
    [SerializeField] private Game _game;

    private bool _isRebornButtonClicked;

    private void OnEnable()
    {
        _clickToContinueButton.onClick.AddListener(OnClickToContinueButton);
    }

    private void OnDisable()
    {
        _clickToContinueButton.onClick.RemoveListener(OnClickToContinueButton);
    }

    private void OnClickToContinueButton()
    {
        _game.Reborn();
    }
}