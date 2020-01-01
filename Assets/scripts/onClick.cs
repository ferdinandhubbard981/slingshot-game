using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onClick : MonoBehaviour
{
    public Collider2D[] objects;
    public float[,] positions;
    public float[,] scales;
    public float[] zRotations;
    public string[] names;
    void OnMouseDown()
    {
        getData();
        save.SaveLevel(this);
    }

    void getData()
    {
        float screendWidth = Mathf.Abs(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).x) * 2;
         positions = new float[objects.Length, 2];
        scales = new float[objects.Length, 2];
        zRotations = new float[objects.Length];
        names = new string[objects.Length];
        for (int x = 0; x < objects.Length; x++)
        {
            
            
            names[x] = objects[x].gameObject.name;
            positions[x, 0] = objects[x].gameObject.transform.position.x;
            positions[x, 1] = objects[x].gameObject.transform.position.y;
            scales[x, 0] = objects[x].gameObject.transform.localScale.x;
            scales[x, 1] = objects[x].gameObject.transform.localScale.y;
            zRotations[x] = objects[x].gameObject.transform.rotation.eulerAngles.z;
            



        }
    }
}
