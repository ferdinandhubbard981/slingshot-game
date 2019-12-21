using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcollider : MonoBehaviour
{
    public GameObject wallPrefab;
    GameObject test;
    BoxCollider2D boxCollider;
    void Start()
    {
        test = Instantiate(wallPrefab, new Vector3(-6f, 0, 1), Quaternion.identity) as GameObject;
        test.transform.localScale = new Vector3(5, 1);
        boxCollider = test.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(test.transform.localScale.x, test.transform.localScale.y);
        Check();
        

        test = Instantiate(wallPrefab, new Vector3(6f, 0, 1), Quaternion.identity) as GameObject;
        test.transform.localScale = new Vector3(5, 1);
        Check();



    }

    void Check()
    {
        Collider2D newObstacleCollider = test.GetComponent<Collider2D>();
        List<Collider2D> intersections = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        if (Physics2D.OverlapCollider(newObstacleCollider, contactFilter.NoFilter(), intersections) > 0)
        {
            Debug.Log(intersections.Count);
            Debug.Log("name: " + intersections[0].gameObject.name);
            Destroy(test);
            
        }
        else
        {
            boxCollider.size = new Vector2(1, 1);
        }
    }

}
