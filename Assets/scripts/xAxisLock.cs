using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xAxisLock : MonoBehaviour
{
    public Transform vertical;

    void Update()
    {
        
        transform.localScale = new Vector3(0.1f / vertical.localScale.x ,transform.localScale.y, 1);
    }
}
