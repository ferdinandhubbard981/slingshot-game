using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class movement : MonoBehaviour
{
    float touchBaseDistance;
    List<SpriteRenderer> dotSpriteRenderer;
    public GameObject dotObject;
    public GameObject basepoint;
    List<GameObject> trajectoryPoints;
    public int numOfTrajectoryPoints;
    Vector2 basepointposition;
    public TimeManager timeManager;
    private float buffer = 0.6f;
    private float leftConstraint;
    float rightConstraint;
    public static movement instance;
    Vector2 direction;
    public cameramovement cameraFollow;

    public float hopModifier;
    private bool isPressed = false;
    Vector2 touchposition;
    Rigidbody2D rb;
    bool contact;
    Touch touch;
    private void Awake()
    {
        dotSpriteRenderer = new List<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        leftConstraint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10)).x;
        rightConstraint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -10)).x;
        instance = this;
        contact = true;

        trajectoryPoints = new List<GameObject>();
        //TrajectoryPoints are instatiated
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            GameObject dot = Instantiate(dotObject);

            dotSpriteRenderer.Insert(i, dot.GetComponent<SpriteRenderer>());
            dotSpriteRenderer[i].enabled = false;
            //SpriteRenderer dotrenderer = dot.GetComponent<SpriteRenderer>();
            //dotrenderer.enabled = false;
            trajectoryPoints.Insert(i, dot);
        }

    }
    void Update()
    {
        if (rb.position.x < leftConstraint - buffer)
        {
            rb.position = new Vector2(rightConstraint + buffer, rb.position.y);
        }
        else if (rb.position.x > rightConstraint + buffer)
        {
            rb.position = new Vector2(leftConstraint - buffer, rb.position.y);
        }




        //contact 
        if (contact && Input.touchCount > 0)
        {

            touch = Input.GetTouch(0);
            if (/*touch.phase == TouchPhase.Began*/ !isPressed)
            {
                TouchDown();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                TouchUp();
            }
        }

        if (isPressed)
        {
            DragBall();
        }
    }

    private void TouchDown()
    {
        touchposition = Camera.main.ScreenToWorldPoint(touch.position);
        isPressed = true;
        basepoint.transform.position = touchposition;
        basepointposition = new Vector2(basepoint.transform.position.x, basepoint.transform.position.y);
        timeManager.SlowDownTime();
        cameraFollow.cameraMovement = false;
    }


    private void TouchUp()
    {
        contact = false;
        isPressed = false;
        timeManager.SpeedUpTime();
        rb.velocity = direction * -hopModifier * touchBaseDistance;
        EnableDotRenderer(false);
        cameraFollow.cameraMovement = true;
    }

    private void DragBall()
    {        
        touchposition = Camera.main.ScreenToWorldPoint(touch.position);
        direction = (touchposition - basepointposition).normalized;
        trajectoryLine();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            contact = true;
        }
        else if (collision.gameObject.tag == "kill")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void trajectoryLine()
    {
        touchBaseDistance = Mathf.Pow(Mathf.Pow((touchposition.y - basepointposition.y), 2) + Mathf.Pow((touchposition.x - basepointposition.x), 2), 0.5f);
        if (touchBaseDistance > 5)
        {
            touchBaseDistance = 5;
        }
        else if (touchBaseDistance < 1)
        {
            touchBaseDistance = 1;
        }
        float subDistance = touchBaseDistance / numOfTrajectoryPoints;
        //float yDistance = Mathf.Pow((touchposition.y + basepointposition.y), 2);
        //float xDistance = Mathf.Pow((touchposition.x + basepointposition.x), 2);




        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            Vector2 pos = rb.position - (direction * subDistance * (i + 1));
            //Vector2 pos = new Vector2(basepointposition.x + subDistance * (i + 1) * direction.x, basepointposition.y + subDistance * (i + 1) * direction.y);
            trajectoryPoints[i].transform.position = pos;
            dotSpriteRenderer[i].enabled = true;
        }
        
    }
    void EnableDotRenderer(bool state)
    {
        if (state)
        {
            foreach (SpriteRenderer dotRenderer in dotSpriteRenderer)
            {
                dotRenderer.enabled = true;
            }
        }
        else
        {
            foreach (SpriteRenderer dotRenderer in dotSpriteRenderer)
            {
                dotRenderer.enabled = false;
            }
        }
    }
}
    


