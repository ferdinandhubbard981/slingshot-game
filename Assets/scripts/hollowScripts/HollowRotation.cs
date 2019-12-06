using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class HollowRotation : MonoBehaviour
{
    bool safeToTransform = false;
    float zAxisRotation;
    public GameObject verticalLeft;
    public GameObject verticalRight;
    public GameObject horizontalUp;
    public GameObject horizontalDown;
    AnchorX xAnchorLeft; // vertical lines    
    AnchorX xAnchorRight; // vertical lines
    xAxisLock xLockLeft; // vertical lines
    xAxisLock xLockRight; // vertical lines
    AnchorY yAnchorDown; //horizontal lines
    AnchorY yAnchorUp; //horizontal lines
    yAxisLock yLockDown; // horizontal lines
    yAxisLock yLockUp; // horizontal lines



    private void Awake()
    {
        yAnchorDown = horizontalDown.GetComponent<AnchorY>();
        xAnchorLeft = verticalLeft.GetComponent<AnchorX>();
        yLockDown = horizontalDown.GetComponent<yAxisLock>();
        xLockLeft = verticalLeft.GetComponent<xAxisLock>();
        yAnchorUp = horizontalUp.GetComponent<AnchorY>();
        xAnchorRight = verticalRight.GetComponent<AnchorX>();
        yLockUp = horizontalUp.GetComponent<yAxisLock>();
        xLockRight = verticalRight.GetComponent<xAxisLock>();
    }
    void Update()
    {
        Debug.Log("working");
        if (!safeToTransform)
        {
            zAxisRotation = transform.rotation.eulerAngles.z;
            transform.rotation = new Quaternion();
            safeToTransform = true;
        }
        yAnchorDown.yPos();
        xAnchorLeft.xPos();
        yLockDown.yLock();
        xLockLeft.xLock();
        yAnchorUp.yPos();
        xAnchorRight.xPos();
        yLockUp.yLock();
        xLockRight.xLock();
    }
    private void LateUpdate()
    {
        if (safeToTransform)
        {
            transform.rotation = Quaternion.Euler(0, 0, zAxisRotation);
            safeToTransform = false;
        }
        
    }
}
