using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class cameramovement : MonoBehaviour {
    GameObject playerPrefab;
    public Transform player;
    public bool cameraMovement;
    public float smoothSpeed;
    public Vector3 offset;
    
    void Awake()
    {
        cameraMovement = true;
    }
    void FixedUpdate()
    {
        if (player.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).y)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if ((player.position.y + 5 > transform.position.y/* || player.position.y + 5 < transform.position.y*/) & cameraMovement)
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, player.position.y + offset.y, player.position.z + offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            //transform.LookAt(player.position);
        }

	}
}
