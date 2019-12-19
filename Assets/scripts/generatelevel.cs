using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatelevel : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject hollowPrefab;
    float randomXPos, randomYPos, randomXScale, randomYScale, randomZRotation;
    float screenwidth;
    
    float subsection;
    void Start()
    {
        subsection = 5;
        screenwidth = Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x) * 2;
        ObstacleGenerator();
    }

    void ObstacleGenerator()
    {
        int failcount = 0;
        int layerId = 8;
        int layerMask = 1 << layerId;
        float subsectionOverlapArea = 0;
        int x = 0;
        GameObject currentObstacle = new GameObject();
        RandomNumbers(x);
        Generateobstacle(ref x, randomXPos, randomYPos, randomXScale, randomYScale, ref currentObstacle);
        Collider2D[] objectsInSubsection = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, 5), new Vector2(screenwidth / 2, 30));
        while (CalculateAreaOverlap(objectsInSubsection, screenwidth * 30) < 0.2 && failcount < 10)
        {
            while (subsection < 30)
            {
                Debug.Log("subsection: " + subsection);
                while (subsectionOverlapArea < 0.2)
                {                  
                    RandomNumbers(x);
                    Check(currentObstacle, ref x, layerMask);

                    
                    Generateobstacle(ref x, randomXPos, randomYPos, randomXScale, randomYScale, ref currentObstacle);
                    subsectionOverlapArea = CalculateAreaOverlap(objectsInSubsection, 5 * screenwidth);
                    //Debug.Log(subsectionOverlapArea);

                    objectsInSubsection = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, subsection), new Vector2(screenwidth / 2, subsection + 5), layerMask);
                    //Debug.Log(objectsInSubsection.Length);
                    failcount++;
                }
                failcount++;
                subsectionOverlapArea = 0;
                subsection += 5;

            }

        }


        if (failcount > 10)
        {
            Debug.Log("failed");
        }
    }

    void Generateobstacle(ref int namenum, float xpos, float ypos, float xscale, float yscale, ref GameObject currentObstacle)
    {
        currentObstacle = Instantiate(wallPrefab, new Vector3(xpos, ypos), Quaternion.identity);
        currentObstacle.name = "wall" + namenum;
        currentObstacle.transform.localScale = new Vector3(xscale, yscale, 1);
        namenum++;
    }

    void RandomNumbers(int x)
    {
        Random r = new Random();
        randomXPos = Random.Range(-screenwidth / 2, screenwidth / 2);
        randomYPos = Random.Range(subsection, subsection + 5);
        randomXScale = Random.Range(1f, 8);
        randomYScale = Random.Range(1, 10 / randomXScale);
        //Debug.Log(x + " pos: " + randomXPos.ToString() + " " + randomYPos.ToString() + " scale: " + randomXScale.ToString() + " " + randomYScale.ToString());
    }
    void Check(GameObject currentObstacle, ref int x, int layerMask)
    {
        Collider2D newObstacleCollider = currentObstacle.GetComponent<Collider2D>();
        List<Collider2D> intersections = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        
        if (Physics2D.OverlapCollider(newObstacleCollider, contactFilter.NoFilter(), intersections) > 0)
        {
            Debug.Log(intersections.Count);
            Debug.Log("name: " + intersections[0].gameObject.name);
            Destroy(currentObstacle);
            x -= 1;
        }
    }
    float CalculateAreaOverlap(Collider2D[] objects, float area)
    {
        
        float totalArea = 0;
        foreach (Collider2D item in objects)
        {
            totalArea += item.gameObject.transform.lossyScale.x * item.gameObject.transform.lossyScale.y;
        }
        return totalArea / area;
    }
}
