using UnityEngine;
using UnityEngine.UI;

public class ContinueGameScreen : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private CanvasFade _canvasFade;
    [SerializeField] private Game _game;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(OnClickToContinueButton);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(OnClickToContinueButton);
    }

    private void OnClickToContinueButton()
    {
        _game.ContunueAfterReborn();
    }
}