using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxmake : MonoBehaviour
{
    public GameObject wallPrefab;

    public GameObject instantiate()
    {
        GameObject box = Instantiate(wallPrefab);
        return box;
    }
}
