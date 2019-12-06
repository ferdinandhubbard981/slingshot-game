using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AnchorY : MonoBehaviour
{
    public bool positive;
    public Transform target;
    public void yPos()
    {
        
        if (positive)
        {
            float desiredYPos = target.position.y + target.lossyScale.y / 2;
            transform.position = new Vector3(transform.position.x, desiredYPos);
        }
        else
        {
            float desiredYPos = target.position.y - target.lossyScale.y / 2;
            transform.position = new Vector3(transform.position.x, desiredYPos);
        }

    }

}


