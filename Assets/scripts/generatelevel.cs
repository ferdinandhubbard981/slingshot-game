using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatelevel : MonoBehaviour
{
    public GameObject obstacle;
    float randomxpos, randomypos, randomxscale, randomyscale;
    float screenwidth;
    GameObject newObstacle;
    float obstaclegap;
    void Start()
    {
        newObstacle = obstacle;
        obstaclegap = 3;
        screenwidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
        //ObstacleGenerator();
    }

    void ObstacleGenerator()
    {
        for (int x = 0; x <= 200; x++)
        {
            RandomNumbers(x);
            if (Checks(newObstacle, ref x))
            {
                Generateobstacle(x, randomxpos, randomypos, randomxscale, randomyscale);
            }

        }


    }

    void Generateobstacle(int namenum, float xpos, float ypos, float xscale, float yscale)
    {
        newObstacle = GameObject.Instantiate(obstacle);
        newObstacle.name = "obstacle" + namenum;
        newObstacle.transform.position = new Vector3(xpos, ypos);
        newObstacle.transform.localScale = new Vector3(xscale, yscale, 1);
    }

    int RandomNumbers(int x)
    {
        randomxpos = Random.Range(0, screenwidth);
        randomypos = Random.Range(x * obstaclegap, x * obstaclegap + obstaclegap);
        do
        {
            randomxscale = Random.Range(0.1f, 8);
            randomyscale = Random.Range(0.1f, 12);
            
        } while (randomxscale * randomyscale > 16);
       
        return x;
    }
    bool Checks(GameObject newobstacle, ref int x)
    {
        Collider[] overlapingobjects = Physics.OverlapBox(newobstacle.transform.position, newobstacle.transform.localScale / 2, Quaternion.identity);
        foreach (var colliders in overlapingobjects)
        {
            Destroy(colliders.gameObject);
            x -= 1;
        }
        return true;
    }

}
