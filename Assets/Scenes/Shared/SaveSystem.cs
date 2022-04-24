using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string playerFileName = "/player.paa";

    public static void SavePlayer(GenericPlayer playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + playerFileName;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static GenericPlayer LoadPlayer()
    {
        string path = Application.persistentDataPath + playerFileName;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GenericPlayer data = formatter.Deserialize(stream) as GenericPlayer;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log($"Save file not found at {path}");
            return null;
        }
    }

    public static bool HasExistingPlayerData()
    {
        string path = Application.persistentDataPath + playerFileName;
        return (File.Exists(path));
    }
}
