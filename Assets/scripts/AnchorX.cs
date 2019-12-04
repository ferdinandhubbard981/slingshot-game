using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorX : MonoBehaviour
{
    public bool positive;
    public Transform target;
    //public Vector3 localPosition;

    void Update()
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
        


















        /*if (target == null) return;
        Vector3 desiredPos = target.position - transform.TransformVector(localPosition);
        transform.position = new Vector3(desiredPos.x, transform.position.y);
        */
    }
}
