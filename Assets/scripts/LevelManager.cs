using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelManager : MonoBehaviour
{
    string path = Directory.GetCurrentDirectory() + "\\levels\\LevelNum";
    int levelNum = 1;
    public GameObject levelLoader;
    LoadLevel loadScript;
    int LevelIndex;
    private void Start()
    {
        int levelIncrement = 0;
        loadScript = levelLoader.GetComponent<LoadLevel>();
        do
        {
            ReadLevelNum(levelIncrement);
            levelIncrement++;
        } while (File.Exists(Directory.GetCurrentDirectory() + "\\levels\\level_" + levelNum) == false && levelIncrement < 2000);
        loadScript.Load(levelNum);
        Debug.Log("level: " + levelNum);
    }

    public void ReadLevelNum(int increment)
    {
        LevelIndex = 0;
        if (File.Exists(path))
        {
            BinaryReader binReader = new BinaryReader(File.Open(path, FileMode.Open));
            LevelIndex = binReader.ReadInt32();
            binReader.Close();
        }
        else
        {
            LevelIndex = -1;
            WriteLevelNum();
            LevelIndex = 0;
        }

        levelNum = levelRanker.GetNextLevel(LevelIndex + increment);
        
        
        
    }
    public void WriteLevelNum()
    {
        BinaryWriter binWriter = new BinaryWriter(File.Create(path));
        binWriter.Write(LevelIndex + 1);
        binWriter.Close();
        
    }
}
