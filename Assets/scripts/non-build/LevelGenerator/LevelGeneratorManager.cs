using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneratorManager : MonoBehaviour
{
    public float[,] positions;
    public float[,] scales;
    public float[] zRotations;
    public string[] names;
    public int levelHeight;
    public int Save(Collider2D[] objects, int passedLevelHeight)
    {
        GetData(objects, passedLevelHeight);
        return save.SaveLevel(this);
    }

    void GetData(Collider2D[] objects, int passedLevelHeight)
    {
        positions = new float[objects.Length, 2];
        scales = new float[objects.Length, 2];
        zRotations = new float[objects.Length];
        names = new string[objects.Length];
        levelHeight = passedLevelHeight;
        for (int x = 0; x < objects.Length; x++)
        {


            names[x] = objects[x].gameObject.name;
            positions[x, 0] = objects[x].gameObject.transform.position.x;
            positions[x, 1] = objects[x].gameObject.transform.position.y;
            scales[x, 0] = objects[x].gameObject.transform.localScale.x;
            scales[x, 1] = objects[x].gameObject.transform.localScale.y;
            zRotations[x] = objects[x].gameObject.transform.rotation.eulerAngles.z;

        }
        
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
