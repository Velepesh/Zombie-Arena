using UnityEngine;

public class LocalizationSaver : MonoBehaviour, ISaver
{
    [SerializeField] private LocalizationSelector _selector;

    readonly string _savePath = "/localizatio.json";

    private IDataService _dataService;
    private LocalisationData _data;

    private void Awake()
    {
        _dataService = new JsonDataService();
        LoadData();

        _selector.ChangeLocal(_data.ID);
    }

    private void OnEnable()
    {
        _selector.LocalizationChanged += OnLocalizationChanged;
    }

    private void OnDisable()
    {
        _selector.LocalizationChanged -= OnLocalizationChanged;
    }

    public void SaveData<LocalisationData>(string path, LocalisationData data)
    {
        _dataService.SaveData(path, data);
    }

    public void LoadData()
    {
        _data = _dataService.LoadData<LocalisationData>(_savePath);

        if (_data == null)
            _data = new LocalisationData();
    }

    private void OnLocalizationChanged(int localID)
    {
        _data.ID = localID;
        SaveData(_savePath, _data);
    }
}