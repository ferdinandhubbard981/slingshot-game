﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatelevel : MonoBehaviour
{
    public GameObject generatedLevelManager;
    LevelGeneratorManager saveScript;
    public GameObject[] prefabs;
    public float skewIntensity = 1;
    float randomXPos, randomYPos, randomXScale, randomYScale, randomZRotation, hollowXConstant, hollowYConstant, skew;
    public int levelHeight, requiredWallsPerSubsection = 1;
    public float areaFill = 0.2f;
    float screenwidth;
    GameObject currentObstacle;
    BoxCollider2D boxCollider;
    float subsection;
    void Start()
    {
        saveScript = generatedLevelManager.GetComponent<LevelGeneratorManager>();
        areaFill = Random.Range(areaFill - 0.05f, areaFill + 0.05f);
        hollowXConstant = 0.1724f;
        hollowYConstant = 0.1697f;
        subsection = 5;
        screenwidth = Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x) * 2;
        Collider2D[] objects = ObstacleGenerator();
        Debug.Log("num " + objects.Length);
        
        saveScript.objects = objects;
    }

    Collider2D[] ObstacleGenerator()
    {
        int wallsInSubsection = 0;
        levelHeight = Random.Range(6, 13) * 5;
        Instantiate(prefabs[2], new Vector3(0, levelHeight), Quaternion.identity);

        int failcount = 0;
        int layerId = 8;
        int layerMask = 1 << layerId;
        float subsectionOverlapArea;
        int x = 0;  
        
        Collider2D[] objectsInSection = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, 5), new Vector2(screenwidth / 2, levelHeight + 1));
        while (CalculateAreaOverlap(objectsInSection, screenwidth * (levelHeight - 5)) < areaFill && failcount < 300000)
        {
            while (subsection < levelHeight)
            {
                subsectionOverlapArea = 0;
                wallsInSubsection = 0;
                //Debug.Log("subsection: " + subsection);
                while ((subsectionOverlapArea < areaFill || wallsInSubsection < requiredWallsPerSubsection) && failcount < 300000)
                {
                    if (wallsInSubsection < requiredWallsPerSubsection && subsectionOverlapArea >= areaFill)
                    {
                        RemoveObjectsArray(objectsInSection);
                    }
                    RandomNumbers(x);                  
                    Generateobstacle(ref x, randomXPos, randomYPos, randomXScale, randomYScale, ref currentObstacle);
                    if (Check(ref x, layerMask) && currentObstacle.name == "wall")
                    {
                        wallsInSubsection++;
                    }
                    

                    subsectionOverlapArea = CalculateAreaOverlap(objectsInSection, 5 * screenwidth);
                    //Debug.Log(subsectionOverlapArea);

                    objectsInSection = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, subsection), new Vector2(screenwidth / 2, subsection + 5), layerMask);
                    //Debug.Log(objectsInSubsection.Length);
                    failcount++;
                }
                failcount++;
                subsection += 5;

            }
            failcount++;           
            Collider2D[] objectsOutOfBounds = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, 0), new Vector2(-screenwidth / 2 - 5, levelHeight), layerMask);
            RemoveObjectsArray(objectsOutOfBounds);// to the right
            objectsOutOfBounds = Physics2D.OverlapAreaAll(new Vector2(screenwidth / 2, 0), new Vector2(screenwidth / 2 + 5, levelHeight), layerMask);
            RemoveObjectsArray(objectsOutOfBounds); // to the left
            objectsInSection = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, 5), new Vector2(screenwidth / 2, levelHeight), layerMask);
            subsection = 5;
        }

        ReplaceHollow(objectsInSection);
        if (failcount > 100000)
        { 
        
            Debug.Log("failed");
        }
        objectsInSection = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, 5), new Vector2(screenwidth / 2, levelHeight), layerMask);
        return objectsInSection;
    }

    void Generateobstacle(ref int namenum, float xpos, float ypos, float xscale, float yscale, ref GameObject currentObstacle)
    {
        
        currentObstacle = Instantiate(prefabs[0], new Vector3(xpos, ypos), Quaternion.Euler(0,0,randomZRotation));
        currentObstacle.name = prefabs[Random.Range(0, 2)].name;
        
        currentObstacle.transform.localScale = new Vector3(xscale, yscale);//check problem <<
        boxCollider = currentObstacle.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(currentObstacle.transform.localScale.x, currentObstacle.transform.localScale.y);
        namenum++;
    }

    void RandomNumbers(int x)
    {
        randomXScale = 0;
        randomYScale = 0;
        //skew = Random.Range(0, 3) * skewIntensity;
        skew = 1;
        randomXPos = Random.Range(-screenwidth / 2, screenwidth / 2);
        randomYPos = Random.Range(subsection, subsection + 5);
        while (randomXScale < 1 || randomXScale > 10)
        {
            randomXScale = Random.Range(1, 10) * skew;
        }
        while (randomYScale < 1 || randomYScale > 10 / randomXScale)
        {
            randomYScale = Random.Range(1, 20 / randomXScale) / skew;
        }
        
        randomZRotation = Random.Range(1, 13) * 15;
        //Debug.Log(x + " pos: " + randomXPos.ToString() + " " + randomYPos.ToString() + " scale: " + randomXScale.ToString() + " " + randomYScale.ToString());
    }
    bool Check(ref int x, int layerMask)
    {
        Collider2D newObstacleCollider = currentObstacle.GetComponent<Collider2D>();
        List<Collider2D> intersections = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        //Debug.Log(currentObstacle.name);
        if (Physics2D.OverlapCollider(newObstacleCollider, contactFilter.NoFilter(), intersections) > 0)
        {
            //Debug.Log(intersections.Count);
            //Debug.Log("name: " + intersections[0].gameObject.name);
            Destroy(currentObstacle);
            x -= 1;
            return false;
        }
        else
        {
            boxCollider.size = new Vector2(1, 1);
            return true;
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

    void ReplaceHollow(Collider2D[] objectsInSection)
    {
        foreach (Collider2D collider in objectsInSection)
        {
            Debug.Log(collider.gameObject.name);
            if (collider.gameObject.name == "hollow")
            {
                Vector3 position = collider.gameObject.transform.position;
                Vector3 scale = new Vector3(collider.gameObject.transform.localScale.x * hollowXConstant, collider.gameObject.transform.localScale.y * hollowYConstant);
                Quaternion rotation = collider.gameObject.transform.rotation;
                Destroy(collider.gameObject);
                currentObstacle = Instantiate(prefabs[1], position, rotation);
                currentObstacle.transform.localScale = scale;
            }
        }


    }
    void RemoveObjectsArray(Collider2D[] list)
    {
        foreach (Collider2D item in list)
        {           
            Destroy(item.gameObject);
        }
    }
}
