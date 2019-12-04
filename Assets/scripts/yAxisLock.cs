using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yAxisLock : MonoBehaviour
{
    public Transform horizontal;

    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, 0.1f / horizontal.localScale.y, 1);
    }
}
