using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramovement : MonoBehaviour {

    public Transform player;
    public bool cameraMovement;
    public float smoothSpeed;
    public Vector3 offset;
    // Update is called once per frame
    private void Awake()
    {
        cameraMovement = true;
    }
    void FixedUpdate ()
    {
        if (player.position.y + 5 > transform.position.y & cameraMovement)
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, player.position.y + offset.y, player.position.z + offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            //transform.LookAt(player.position);
        }

	}
}
