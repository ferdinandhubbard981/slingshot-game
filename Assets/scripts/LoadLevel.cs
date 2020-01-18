using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public GameObject[] prefabs;
    public int levelNum;
    void Start()
    {
        string path = System.IO.Directory.GetCurrentDirectory() + "\\levels\\level_" + levelNum.ToString();
        while (System.IO.File.Exists(path) == false && levelNum < 2000)
        {
            
            levelNum++;
            path = System.IO.Directory.GetCurrentDirectory() + "\\levels\\level_" + levelNum.ToString();
        }
        Debug.Log("level num is " + levelNum.ToString());
        if (levelNum > 2000)
        {
            Debug.Log("failed loading");
        }
        Load();
    }
    public void Load()
    {

        levelData data = save.LoadLevel(levelNum);
        float[,] positions = data.positions;
        float[,] scales = data.scales;
        float[] zRotations = data.zRotations;
        string[] names = data.names;
        for (int x = 0; x < names.Length; x++)
        {
            if (!(names[x] == "hollow"))
            {
                Generateobstacle(prefabs[0], positions[x, 0], positions[x, 1], scales[x, 0], scales[x, 1], zRotations[x]);
            }          
            else //if (names[x] == "hollow(Clone)")
            {
                Debug.Log("hollow");
                Generateobstacle(prefabs[1], positions[x, 0], positions[x, 1], scales[x, 0], scales[x, 1], zRotations[x]);
            }



        }

        void Generateobstacle(GameObject prefab, float xpos, float ypos, float xscale, float yscale, float zRotation)
        {
            
            GameObject currentObstacle = Instantiate(prefab, new Vector3(xpos, ypos), Quaternion.Euler(0, 0, zRotation));
            currentObstacle.name = prefab.name;
            currentObstacle.transform.localScale = new Vector3(xscale, yscale);
            /*BoxCollider2D boxCollider = currentObstacle.GetComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(currentObstacle.transform.localScale.x, currentObstacle.transform.localScale.y);*/
        }
    }
}
