using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class save
{
    public static void SaveLevel(LevelGeneratorManager manager)
    {
        int levelNum = 1;
        string path = Directory.GetCurrentDirectory() + "\\levels\\level_" + levelNum.ToString();
        while (File.Exists(path))
        {
            levelNum += 1;
            path = Directory.GetCurrentDirectory() + "\\levels\\level_" + levelNum.ToString();
        }
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        levelData data = new levelData(manager);
        formatter.Serialize(stream, data);
        stream.Close();


    }

    public static levelData LoadLevel(int levelNum)
    {        
        string path = Directory.GetCurrentDirectory() + "\\levels\\level_" + levelNum.ToString();
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
