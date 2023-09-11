using UnityEngine;

public class BackgroundMusicSaver : MonoBehaviour, ISaver
{
    [SerializeField] private BackgroundMusic _backgroundMusic;

    readonly string _savePath = "/backgroundMusic.json";

    private IDataService _dataService;
    private BackgroundMusicData _data;

    private void Awake()
    {
        _dataService = new JsonDataService();
        LoadData();

        _backgroundMusic.Init(_data.IsExist);
    }

    private void OnEnable()
    {
        _backgroundMusic.Inited += OnInited;
        _backgroundMusic.Disabled += OnDisabled;
    }

    private void OnDisable()
    {
        _backgroundMusic.Inited -= OnInited;
        _backgroundMusic.Disabled -= OnDisabled;
    }

    public void SaveData<BackgroundMusicData>(string path, BackgroundMusicData data)
    {
        _dataService.SaveData(path, data);
    }

    public void LoadData()
    {
        _data = _dataService.LoadData<BackgroundMusicData>(_savePath);

        if (_data == null)
            _data = new BackgroundMusicData();
    }

    private void OnInited()
    {
        _data.IsExist = true;
        SaveData(_savePath, _data);
    }

    private void OnDisabled()
    {
        _data.IsExist = false;
        SaveData(_savePath, _data);
    }
}