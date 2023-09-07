public interface ISaver
{
    void SaveData<T>(string path, T data);
    void LoadData();
}