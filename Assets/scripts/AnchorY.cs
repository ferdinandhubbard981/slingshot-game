using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnchorY : MonoBehaviour
{
    public bool positive;
    public Transform target;
    //public Vector3 localPosition;

    void Update()
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
        Debug.Log(target.lossyScale.y);
        Debug.Log((target.position).y);



        /*if (target == null) return;
        Vector3 desiredPos = target.position - transform.TransformVector(localPosition);
        transform.position = new Vector3(transform.position.x, desiredPos.y);
        */
    }

    /*void OnDrawGizmos()
    {
        if (!enabled) return;
        var pos = transform.TransformPoint(localPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, HandleUtility.GetHandleSize(pos) * .05f);   
    }*/
}


