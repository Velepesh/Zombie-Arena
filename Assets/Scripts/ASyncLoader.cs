using UnityEngine;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [SerializeField] private int _gameSceneID;

    private void Start()
    {
        SceneManager.LoadSceneAsync(_gameSceneID);
    }
}