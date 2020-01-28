using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Testcameramovement : MonoBehaviour {
    public GameObject player;
    public bool cameraMovement;
    public float smoothSpeed;
    public Vector3 offset;
    movement movementScript;
    
    void Start()
    {     
        movementScript = player.GetComponent<movement>();
        cameraMovement = true;
    }
    void FixedUpdate()
    {
        if (player.transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).y)
        {
            //movementScript.KillPlayer();
        }
        else if ((player.transform.position.y + 5 > transform.position.y || Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).y + 2 > player.transform.position.y) & cameraMovement)
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            //transform.LookAt(player.position);
        }

	}   
    
    
}
