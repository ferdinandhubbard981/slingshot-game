using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

public static class levelRanker
{
    static string path = Directory.GetCurrentDirectory() + "\\LevelRanker";
    public static void AddToLevelList(int levelHeight, float hollowDensity, int levelNumber)
    {
        float levelDifficulty = GetLevelDifficulty(levelHeight, hollowDensity);
        BinaryFormatter formatter = new BinaryFormatter();

        
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            var dict = formatter.Deserialize(stream) as Dictionary<int, float>;
            try
            {

                dict.Add(levelNumber, levelDifficulty);
            }
            catch (System.Exception)
            {
                dict.Add(levelNumber, levelDifficulty + 0.001f);
                throw;
            }
            Dictionary<int, float> sortedDict = (from entry in dict orderby entry.Value ascending select entry).ToDictionary(i => i.Key, i => i.Value);

            stream.Close();
            stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, sortedDict);
            stream.Close();
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            var dict = new Dictionary<int, float>();
            dict.Add(levelNumber, levelDifficulty);
            formatter.Serialize(stream, dict);
            stream.Close();
        }
        Debug.Log("level number: " + levelNumber + " difficulty: " + levelDifficulty);


    }
    static float GetLevelDifficulty(int levelHeight, float hollowDensity)
    {
        return levelHeight / 60 * 0.3f + hollowDensity * 0.7f;
    }



    static public int GetNextLevel(int index)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            var dict = formatter.Deserialize(stream) as Dictionary<int, float>;
            return dict.ElementAt(index).Key;
        }
        else
        {
            return -1;
        }
    }

   
}
