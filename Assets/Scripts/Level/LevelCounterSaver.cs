using UnityEngine;

public class LevelCounterSaver : MonoBehaviour, ISaver
{
    [SerializeField] private LevelCounter _levelCounter;

    readonly string _savePath = "/level.json";

    private IDataService _dataService;
    private LevelData _data;

    public string LevelText => _data.Level.ToString();


    private void Awake()
    {
        _dataService = new JsonDataService();
        LoadData();

        _levelCounter.Init(_data.Level);
    }

    private void OnEnable()
    {
        _levelCounter.LevelIncreased += OnLevelIncreased;
    }

    private void OnDisable()
    {
        _levelCounter.LevelIncreased -= OnLevelIncreased;
    }

    private void OnLevelIncreased(int level)
    {
        _data.Level = level;
        SaveData(_savePath, _data);
    }

    public void SaveData<LevelData>(string path, LevelData data)
    {
        _dataService.SaveData(path, data);
    }

    public void LoadData()
    {
        _data = _dataService.LoadData<LevelData>(_savePath);

        if (_data == null)
            _data = new LevelData();
    }
}