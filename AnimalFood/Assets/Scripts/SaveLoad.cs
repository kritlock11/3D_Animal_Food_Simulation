using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad: MonoBehaviour
{
    private static string _dataPath = Application.persistentDataPath + "/saves/";

    public static void Save<T>(T value, string key)
    {
        Directory.CreateDirectory(_dataPath);

        var bf = new BinaryFormatter();

        using (var fileStream = new FileStream(_dataPath + key+".txt", FileMode.Create))
        {
            bf.Serialize(fileStream, value);
        }
    }
    public static T Load<T>(string key)
    {
        var bf = new BinaryFormatter();
        T returnValue = default;
        using (var fileStream = new FileStream(_dataPath + key + ".txt", FileMode.Open))
        {
            returnValue = (T) bf.Deserialize(fileStream);
        }

        return returnValue;
    }

    public static bool FileExist(string key)
    {
        var path = _dataPath + key + ".txt";
        return File.Exists(path);
    }
}
