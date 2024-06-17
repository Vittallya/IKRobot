using Newtonsoft.Json;
using System.IO;

public class ConfigurationJsonFileService
{
    private readonly string filePath;
    public ConfigurationJsonFileService(string filePath)
    {
        this.filePath = filePath;
    }

    public void SaveFile(object data, string path = null)
    {
        path ??= filePath;
        using var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        using var streamWriter = new StreamWriter(stream);
        using var jsonWriter = new JsonTextWriter(streamWriter);
        JsonSerializer.Create().Serialize(jsonWriter, data);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>file was created</returns>
    public bool CreateFileIfNotExists()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
            return true;
        }
        return false;
    }


    public T Load<T>(string path = null)
    {
        path ??= filePath;
        using var stream = File.OpenRead(path);
        using var streamReader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(streamReader);
        return JsonSerializer.Create().Deserialize<T>(jsonReader);
    }
}