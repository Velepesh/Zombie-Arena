using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    public static LoadingScreen Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
       // DontDestroyOnLoad(this);
    }

    public async void Load()
    {
        _canvas.enabled = true;

        while (YandexGame.SDKEnabled == false)
            await UniTask.Delay(10, ignoreTimeScale: true);

        await UniTask.Delay(500, ignoreTimeScale: true);

        _canvas.enabled = false;
    }
}