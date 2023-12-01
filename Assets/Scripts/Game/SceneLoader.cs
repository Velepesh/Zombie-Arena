using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Game _game;

    private void OnEnable()
    {
        _game.Restarted += OnRestarted;
    }

    private void OnDisable()
    {
        _game.Restarted += OnRestarted;
    }

    private void OnRestarted()
    {
        Debug.Log("Restar Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}