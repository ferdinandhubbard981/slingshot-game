using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcollider : MonoBehaviour
{
    public GameObject wallPrefab;
    void Start()
    {
        GameObject test = Instantiate(wallPrefab, new Vector3(-1.5f, 0), Quaternion.identity);
        test.transform.localScale = new Vector3(1, 2);
        test.name = "wall1";
        test = Instantiate(wallPrefab, new Vector3(1.5f, 0), Quaternion.identity);
        test.name = "wall2";
        test = Instantiate(wallPrefab);
        test.name = "wall3";
        Collider2D collider = test.GetComponent<BoxCollider2D>();
        List<Collider2D> intersections = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = 1 << 8;
        //contactFilter.useTriggers = false;
        Debug.Log(collider.OverlapCollider(contactFilter, intersections));
        foreach (var box in intersections)
        {
            Debug.Log(box.gameObject.name);
        }
    }
    
}
