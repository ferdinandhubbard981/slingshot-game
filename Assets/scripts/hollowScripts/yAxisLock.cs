using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class yAxisLock : MonoBehaviour
{
    public Transform horizontalScale;

    public void yLock()
    {
        transform.localScale = new Vector3(transform.localScale.x, 0.05f / horizontalScale.localScale.y, 1);
    }
}
