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
        loadScript = levelLoader.GetComponent<LoadLevel>();
        ReadLevelNum();
        loadScript.InitializeLevel(levelNum);
    }

    void ReadLevelNum()
    {
        LevelIndex = 0;
        if (File.Exists(path))
        {
            BinaryReader binReader = new BinaryReader(File.Open(path, FileMode.Open));
            LevelIndex = binReader.ReadInt32();
            binReader.Close();
        }
        levelNum = levelRanker.GetNextLevel(LevelIndex);
        
        
    }
    public void WriteLevelNum()
    {
        BinaryWriter binWriter = new BinaryWriter(File.Create(path));
        binWriter.Write(LevelIndex + 1);
        binWriter.Close();
        
    }
}
