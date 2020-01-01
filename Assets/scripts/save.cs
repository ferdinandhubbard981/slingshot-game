using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class save
{
    public static void SaveLevel(onClick onClick)
    {
        int fileNum = 1;
        string path = "D:\\coding\\slingshot-game\\levels\\level_" + fileNum.ToString();
        while (File.Exists(path))
        {
            fileNum += 1;
            path = "D:\\coding\\slingshot-game\\levels\\level_" + fileNum.ToString();
        }
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        levelData data = new levelData(onClick);
        formatter.Serialize(stream, data);
        stream.Close();


    }

    public static levelData LoadLevel(int fileNum)
    {        
        string path = "D:\\coding\\slingshot-game\\levels\\level_" + fileNum.ToString();
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            levelData data = formatter.Deserialize(stream) as levelData;
            stream.Close();
            return data;
        }
        
        else
        {
           Debug.LogError("save file not found in " + path);
            return null;
        }


    }
}
