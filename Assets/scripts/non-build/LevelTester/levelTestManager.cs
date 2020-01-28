using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
public class levelManager : MonoBehaviour
{
    /*
    public GameObject levelLoader;
    LoadLevel loadScript;

    void Awake()
    {
        loadScript = levelLoader.GetComponent<LoadLevel>();
        string path = Directory.GetCurrentDirectory() + "\\levels\\NextNum";
        if (File.Exists(path))
        {
            BinaryReader binReader = new BinaryReader(File.Open(path, FileMode.Open));
            loadScript.levelNum = binReader.ReadInt32();
            binReader.Close();
        }        
    }

    public void MoveLevel(string folderName)
    {
        int movedLevelNum = 1;
        string destinationPath = Directory.GetCurrentDirectory() + "\\" + folderName + "\\level_" + movedLevelNum.ToString();
        (new FileInfo(destinationPath)).Directory.Create();
        while (File.Exists(destinationPath) == true)
        {
            movedLevelNum++;
            destinationPath = Directory.GetCurrentDirectory() + "\\" + folderName + "\\level_" + movedLevelNum.ToString();
        };
        File.Move(Directory.GetCurrentDirectory() + "\\levels\\level_" + loadScript.levelNum, Directory.GetCurrentDirectory() + "\\" + folderName + "\\level_" + movedLevelNum);
        NextLevel();
    }

    public void RemoveLevel()
    {
        MoveLevel("removedLevels");
    }
   

    public void MoveToEasy()
    {
        MoveLevel("easy");
    }
    public void MoveToMedium()
    {
        MoveLevel("medium");
    }
    public void MoveToHard()
    {
        MoveLevel("hard");
    }
    public void NextLevel()
    {
        string path = Directory.GetCurrentDirectory() + "\\levels\\NextNum";
        BinaryWriter binWriter = new BinaryWriter(File.Create(path));
        
        binWriter.Write(loadScript.levelNum += 1);
        binWriter.Close();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }*/
}
