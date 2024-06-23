using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    public static void SavePlayer(PlayerStatus playerStatus)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerStatus);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("저장 파일을 찾을 수 없습니다: " + path);
            return null;
        }
    }
}
