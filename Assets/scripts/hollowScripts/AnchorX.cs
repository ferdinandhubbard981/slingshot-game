using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class AnchorX : MonoBehaviour
{
    public bool positive;
    public Transform target;   
    public void xPos()
    {

        if (positive)
        {
            float desiredXPos = target.position.x + target.lossyScale.x / 2 - transform.lossyScale.x / 2;
            transform.position = new Vector3(desiredXPos, transform.position.y);

        }
        else
        {
            float desiredXPos = target.position.x - target.lossyScale.x / 2 + transform.lossyScale.x / 2;
            transform.position = new Vector3(desiredXPos, transform.position.y);
        }
        
        
    }
}