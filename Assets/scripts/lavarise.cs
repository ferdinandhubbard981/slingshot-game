using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavarise : MonoBehaviour {
    public float speed;
    public bool movement;

    void Start()
    {
        movement = false;
    }
    void Update ()
    {
        if (movement)
        {
            transform.position = new Vector3(0, transform.position.y + speed * Time.deltaTime, 0);
        }    
    }
}
