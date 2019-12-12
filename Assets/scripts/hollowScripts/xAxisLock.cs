using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class xAxisLock : MonoBehaviour
{
    public Transform verticalScale;
    
    public void xLock()
    {
        transform.localScale = new Vector3(0.025f / verticalScale.localScale.x ,transform.localScale.y, 1);
    }
    
}
