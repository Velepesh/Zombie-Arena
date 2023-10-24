using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [SerializeField] private int _gameSceneID;

    private void Start()
    {
        StartCoroutine(LoadLevelAsync());
    }

    private IEnumerator LoadLevelAsync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_gameSceneID);
        while(loadOperation.isDone == false)
        {
            yield return null;
        }
    }
}