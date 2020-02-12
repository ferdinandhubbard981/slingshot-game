using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class generatelevel : MonoBehaviour
{

    //public GameObject captureCam;
    public GameObject generatedLevelManager;
    LevelGeneratorManager saveScript;
    public GameObject[] prefabs;
    public float skewIntensity = 1;
    float randomXPos, randomYPos, randomXScale, randomYScale, randomZRotation, hollowXConstant, hollowYConstant, skew;
    public int levelHeight, requiredWallsPerSection = 1;
    public float areaFill = 0.075f;
    float screenwidth;
    GameObject currentObstacle;
    BoxCollider2D boxCollider;
    float currentSection;
    
    void Start()
    {
        float percentileOfDifficulty = 10, desiredDifficultyValue = 0;

        //levelRanker.GetDifficultyPercentileValue(ref desiredDifficultyValue, percentileOfDifficulty);
        areaFill += Random.Range(-0.05f, 0.05f) / 10;
        saveScript = generatedLevelManager.GetComponent<LevelGeneratorManager>();
        hollowXConstant = 0.1724f;
        hollowYConstant = 0.1697f;
        currentSection = 5;
        screenwidth = Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x) * 2;
        //int requiredVariance = 1;
        //int variance = 0;
        Collider2D[] objectsInLevel;
        /*do
        {
            objects = ObstacleGenerator();
            //GenerateImage();

        } while (variance > requiredVariance);
        */

        //Debug.Log("num " + objects.Length);
        float hollowDensity = 0;
        float wallDensity = 0;
        objectsInLevel = ObstacleGenerator(ref hollowDensity, ref wallDensity);
        if (Input.GetKey(KeyCode.Space) == false)
        {
            int levelNum = saveScript.Save(objectsInLevel, levelHeight);
            levelRanker.AddToLevelList(levelHeight, hollowDensity, wallDensity, levelNum);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    Collider2D[] ObstacleGenerator(ref float hollowDensity, ref float wallDensity)
    {
        int wallsInsection = 0;
        levelHeight = Random.Range(6, 13) * 5;
        Instantiate(prefabs[2], new Vector3(0, levelHeight), Quaternion.identity);
        int failCount = 0;
        int innerFailCount = 0;
        int middleFailCount = 0;
        int outerFailCount = 0;
        int layerId = 8;
        int layerMask = 1 << layerId;
        float sectionOverlapArea;
        int x = 0;

        Collider2D[] collidersInSection = new Collider2D[0];
        Collider2D[] collidersInLevel = new Collider2D[0];
        while (CalculateAreaOverlap(collidersInLevel, screenwidth * (levelHeight - 5)) < areaFill && failCount < 100000)
        {
            while (currentSection < levelHeight)
            {
                float changedAreaFill = areaFill + Random.Range(-0.03f, 0.1f);
                sectionOverlapArea = 0;
                wallsInsection = 0;
                //Debug.Log("subsection: " + subsection);
                while ((sectionOverlapArea < changedAreaFill || wallsInsection < requiredWallsPerSection) && innerFailCount < 100000)
                {
                    //Debug.Log("section overlap area: " + sectionOverlapArea);
                    /*if (sectionOverlapArea >= areaFill && currentSection % 2 == 0)
                    {
                        if (collidersInSection.Length >= requiredWallsPerSection)
                        {
                            Collider2D[] destroyColliders = new Collider2D[requiredWallsPerSection];
                            for (int i = 0; i < requiredWallsPerSection; i++)
                            {
                                destroyColliders[i] = collidersInSection[i];
                            }
                            RemoveColliderObjectsArray(destroyColliders);
                        }
                        else if (collidersInSection.Length > 0)
                        {
                            RemoveColliderObjectsArray(collidersInSection);
                        }
                        
                        
                    }*/
                    RandomNumbers(x);
                    Generateobstacle(ref x, randomXPos, randomYPos, randomXScale, randomYScale, ref currentObstacle);
                    if (Check(ref x, layerMask) && currentObstacle.name == "wall")
                    {
                        wallsInsection++;
                    }

                    collidersInSection = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, currentSection), new Vector2(screenwidth / 2, currentSection + 5), layerMask);
                    //Debug.Log(objectsInSubsection.Length);
                    sectionOverlapArea = CalculateAreaOverlap(collidersInSection, 5 * screenwidth);
                    //Debug.Log(subsectionOverlapArea);                 
                    innerFailCount++;
                    failCount++;
                }
                middleFailCount++;
                currentSection += 5;

            }

            outerFailCount++;
            Collider2D[] collidersOutOfBounds = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, 0), new Vector2(-screenwidth / 2 - 5, levelHeight), layerMask);
            RemoveColliderObjectsArray(collidersOutOfBounds);// to the right
            collidersOutOfBounds = Physics2D.OverlapAreaAll(new Vector2(screenwidth / 2, 0), new Vector2(screenwidth / 2 + 5, levelHeight), layerMask);
            RemoveColliderObjectsArray(collidersOutOfBounds); // to the left
            collidersInLevel = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, 5), new Vector2(screenwidth / 2, levelHeight), layerMask);
            currentSection = 5;
        }
        hollowDensity = GetHollowDensity(collidersInLevel);
        wallDensity = GetWallDensity(collidersInLevel);
        ReplaceHollow(collidersInLevel);
        if (innerFailCount + middleFailCount + outerFailCount > 99999)
        {

            Debug.Log("failed");
            Debug.Log("innerCount" + innerFailCount);
            Debug.Log("middleCount" + middleFailCount);
            Debug.Log("outerCount" + outerFailCount);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (FirstJumpPossible() && WallsWithinRange())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        collidersInLevel = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, 5), new Vector2(screenwidth / 2, levelHeight), layerMask);
        return collidersInLevel;
    }

    void Generateobstacle(ref int namenum, float xpos, float ypos, float xscale, float yscale, ref GameObject currentObstacle)
    {

        currentObstacle = Instantiate(prefabs[0], new Vector3(xpos, ypos), Quaternion.Euler(0, 0, randomZRotation));
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
        randomYPos = Random.Range(currentSection, currentSection + 5);
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
            //Debug.Log(collider.gameObject.name);
            if (collider.gameObject.name == "hollow")
            {
                Vector3 position = collider.gameObject.transform.position;
                Vector3 scale = new Vector3(collider.gameObject.transform.localScale.x * hollowXConstant, collider.gameObject.transform.localScale.y * hollowYConstant);
                Quaternion rotation = collider.gameObject.transform.rotation;
                Destroy(collider.gameObject);
                currentObstacle = Instantiate(prefabs[1], position, rotation);
                currentObstacle.transform.localScale = scale;
                currentObstacle.name = prefabs[1].name;
            }
        }


    }
    void RemoveColliderObjectsArray(Collider2D[] list)
    {
        foreach (Collider2D item in list)
        {
            if (item.gameObject.tag != "win")
            {
                Destroy(item.gameObject);
            }
        }
    }





    void GenerateImage()
    {
        string filename = System.IO.Directory.GetCurrentDirectory() + "\\test.png";
        Screen.SetResolution(750, 8004, true);
        ScreenCapture.CaptureScreenshot(filename);
        Screen.SetResolution(750, 8004, true);

        /*Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //Read the pixels in the Rect starting at 0,0 and ending at the screen's width and height
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filename, bytes);*/
    }

    void LoadPixelArray()
    {

    }

    float GetHollowDensity(Collider2D[] colliders)
    {
        float hollowDensity = 0;
        foreach (Collider2D item in colliders)
        {
            if (item.gameObject.name == "hollow")
            {
                hollowDensity += item.gameObject.transform.localScale.x * item.gameObject.transform.localScale.y;
            }
        }
        return hollowDensity / (levelHeight * screenwidth);
    }

    float GetWallDensity(Collider2D[] colliders)
    {
        float wallDensity = 0;
        foreach (Collider2D item in colliders)
        {
            if (item.gameObject.name == "wall")
            {
                wallDensity += item.gameObject.transform.localScale.x * item.gameObject.transform.localScale.y;
            }
        }
        return wallDensity / (levelHeight * screenwidth);
    }

    Vector2[] setTrajectoryPoints(float angle)
    {
        Vector2 pStartPosition = new Vector2(0, 0.85f);
        float gravityScale = 2;
        Vector2[] trajectoryPoints = new Vector2[30]; 
        Vector2 pVelocity = new Vector2(0.5f, 0.5f) * 5 * 5;
        float timeStep = 0.05f;
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        //float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));

        float fTime = 0;

        fTime += timeStep;
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * gravityScale * fTime * fTime / 2.0f);
            Vector2 pos = new Vector2(pStartPosition.x + dx, pStartPosition.y + dy);
            trajectoryPoints[i] = pos;
            //trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
            //trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += timeStep;
        }
        return trajectoryPoints;
    }

    bool FirstJumpPossible()
    {
        for (int angle = 0; angle <= 180; angle++)
        {

            Vector2[] trajectoryPoints = setTrajectoryPoints(angle);
            angle++;
            for (int point = 0; point < trajectoryPoints.Length - 1; point++)
            {
                RaycastHit2D result = Physics2D.Linecast(trajectoryPoints[point], trajectoryPoints[point + 1]);
                if (result && result.collider.gameObject.tag == prefabs[0].tag)
                {
                    return true;
                }

            }
        }
        return false;
    }
    bool WallsWithinRange()
    {
        bool wallObject;
        for (int x = 0; x < levelHeight - 14; x++)
        {
            Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(-screenwidth / 2, x), new Vector2(screenwidth / 2, x + 14));
            wallObject = false;
            foreach (Collider2D item in colliders)
            {
                if (item.gameObject.tag == prefabs[0].tag)
                {
                    wallObject = true;
                }
            }
            if (!wallObject)
            {
                return false;
            }
        }
        return true;
    }

}
