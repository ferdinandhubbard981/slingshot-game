using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

public static class levelRanker
{
    static string path = Directory.GetCurrentDirectory() + "\\LevelRanker";


    public static void AddToLevelList(int levelHeight, float hollowDensity, float wallDensity, int levelNumber)
    {
        float levelDifficulty = GetLevelDifficulty(hollowDensity, wallDensity);
        BinaryFormatter formatter = new BinaryFormatter();

        
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            var dict = formatter.Deserialize(stream) as Dictionary<int, float>;
            try
            {

                dict.Add(levelNumber, levelDifficulty);
            }
            catch (System.ArgumentException)
            {
                dict[levelNumber] = levelDifficulty;
            }           
            Dictionary<int, float> sortedDict = (from entry in dict orderby entry.Value descending select entry).ToDictionary(i => i.Key, i => i.Value);

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
        Debug.Log("level number: " + levelNumber + "\ndifficulty: " + levelDifficulty + "density: " + hollowDensity);




    }
    public static float GetLevelDifficulty(float hollowDensity, float wallDensity)
    {
        return hollowDensity * 0.7f + wallDensity * 0.2f;
    }





    public static int GetNextLevel(int index)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            var dict = formatter.Deserialize(stream) as Dictionary<int, float>;
            stream.Close();
            Debug.Log(dict.ElementAt(index).Value);
            return dict.ElementAt(index).Key;
        }
        else
        {
            return -1;
        }
    }

    public static void GetDifficultyPercentileValue(ref float difficultyValue,  float percentile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            var dict = formatter.Deserialize(stream) as Dictionary<int, float>;
            stream.Close();
            float interQuartileRange = dict.ElementAt(dict.Count / 4).Value - dict.ElementAt(dict.Count * 3 / 4).Value;
            difficultyValue = Mathf.Abs(interQuartileRange) * percentile / 100f + dict.ElementAt(dict.Count / 4).Value;
        }
        else
        {
            difficultyValue = -1f;
        }

    }

   
}
