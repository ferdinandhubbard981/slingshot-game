using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;

public class TestMovement : MonoBehaviour
{
    float touchBaseDistance;
    List<SpriteRenderer> dotSpriteRenderer;
    public GameObject dotPrefab;
    public GameObject lavaPrefab;
    lavarise lavaScript;
    GameObject lava;
    List<GameObject> trajectoryPoints;
    public int numOfTrajectoryPoints = 10;
    Vector2 basepointposition;
    public TimeManager timeManager;
    private float buffer = 0.6f;
    private float leftConstraint;
    float rightConstraint;
    public static TestMovement instance;
    Vector2 direction;
    //public GameObject cameraHolderPrefab;
    public GameObject cameraHolder;
    cameramovement cameraFollow;
    public float hopModifier = 5;
    private bool isPressed = false;
    Vector2 touchposition;
    Rigidbody2D rb;
    bool contact;
    Touch touch;

    private void Start()
    {   
        lava = Instantiate(lavaPrefab);
        lavaScript = lava.GetComponent<lavarise>();
        cameraFollow = cameraHolder.GetComponent<cameramovement>();
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
            GameObject dot = Instantiate(dotPrefab);

            dotSpriteRenderer.Insert(i, dot.GetComponent<SpriteRenderer>());
            dotSpriteRenderer[i].enabled = false;
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
        basepointposition = new Vector2(touchposition.x, touchposition.y);
        timeManager.SlowDownTime();
        cameraFollow.cameraMovement = false;
    }


    private void TouchUp()
    {
        contact = false;
        isPressed = false;
        timeManager.SpeedUpTime();
        rb.velocity = direction * -hopModifier * touchBaseDistance; //for multiple power levels
        //rb.velocity = direction * -hopModifier; //for no power control
        EnableDotRenderer(false);
        cameraFollow.cameraMovement = true;
        rb.gravityScale = 2;
        lavaScript.movement = true;
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

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "kill")
        {
            //IF REAL BUILD
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            KillPlayer();
        }
        else if (collision.gameObject.tag == "win")
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
        else
        {
            touchBaseDistance = Mathf.Round(touchBaseDistance);
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
    public void KillPlayer()
    {
        Destroy(lava);
        cameraFollow.cameraMovement = false;
        cameraFollow.transform.position = new Vector3(0, 5.85f, -10);
        rb.velocity = new Vector2(0, 0);
        rb.position = new Vector2(0, 0.85f);
        contact = true;
        
        rb.gravityScale = 0;
        //Destroy(gameObject);
        lava = Instantiate(lavaPrefab);

    }
    void LavaAdjust()
    {
        if (transform.position.y - (lava.transform.position.y + lava.transform.localScale.y / 2) > 10)
        {
            Vector3 desiredPosition = new Vector3(0, transform.position.y - (10 + lavaScript.transform.localScale.y / 2), 1);
            Vector3 smoothedPosition = Vector3.Lerp(lava.transform.position, desiredPosition, 0.05f);
            lava.transform.position = smoothedPosition;
        }
    }

}