using UnityEngine;
using UnityEngine.UI;

public class RebornScreen : MonoBehaviour
{
    [SerializeField] private Button _clickToContinueButton;
    [SerializeField] private RebornButton _rebornButton;
    [SerializeField] private CanvasFade _canvasFade;
    [SerializeField] private Game _game;

    private bool _isRebornButtonClicked;

    private void OnEnable()
    {
        _clickToContinueButton.onClick.AddListener(OnClickToContinueButton);
        _rebornButton.RebornButtonClicked += OnRebornButtonClicked;
    }

    private void OnDisable()
    {
        _clickToContinueButton.onClick.RemoveListener(OnClickToContinueButton);
        _rebornButton.RebornButtonClicked -= OnRebornButtonClicked;
    }

    public void ShowScreen()
    {
        if(_isRebornButtonClicked)
            _canvasFade.Show();
    }

    private void OnClickToContinueButton()
    {
        if (_isRebornButtonClicked)
            _game.Reborn();
    }

    private void OnRebornButtonClicked()
    {
        _isRebornButtonClicked = true;
    }
}