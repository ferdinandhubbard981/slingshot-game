using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;

public class movement : MonoBehaviour
{
    float startTime = 0;
    Vector2 startPos = new Vector2(0,0), startVelocity = new Vector2(0, 0);
    float trailTime;
    float touchBaseDistance;
    List<SpriteRenderer> dotSpriteRenderer;
    public GameObject dotPrefab;
    public GameObject lavaPrefab;
    public GameObject levelManager;
    LevelManager levelManagerScript;
    lavarise lavaScript;
    GameObject lava;
    List<GameObject> trajectoryPoints;
    public int numOfTrajectoryPoints;
    Vector2 basepointposition;
    public TimeManager timeManager;
    private float buffer = 0.6f;
    private float leftConstraint;
    float rightConstraint;
    public static movement instance;
    Vector2 direction;
    //public GameObject cameraHolderPrefab;
    public GameObject cameraHolder;
    cameramovement cameraFollow;
    public float hopModifier;
    private bool isPressed = false;
    Vector2 touchposition;
    Rigidbody2D rb;
    TrailRenderer tr;
    bool contact;
    Touch touch;

    //public GameObject trajectoryRenderer;
    //SimulateTrajectory trajectoryScript;

    private void Start()
    {
        tr = GetComponent<TrailRenderer>();
        //trajectoryScript = trajectoryRenderer.GetComponent<SimulateTrajectory>();
        levelManagerScript = levelManager.GetComponent<LevelManager>();
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
        /*if (Time.time - trailTime > 0.2f & Time.time - trailTime < 0.4f)
        {
            Debug.Log("emitting");
            //tr.emitting = true;
            tr.enabled = true;
            trailTime = 0;
        }*/
        //UpdatePlayerPos();
        if (rb.position.x < leftConstraint - buffer)
        {
            ResetTrailRenderer();
            //tr.emitting = false;
            //trailTime = Time.time;
            //tr.enabled = false;
            rb.position = new Vector2(rightConstraint + buffer, rb.position.y);
            //tr.enabled = true;
        }
        else if (rb.position.x > rightConstraint + buffer)
        {
            ResetTrailRenderer();
            //tr.enabled = false;
            //tr.emitting = false;
            //trailTime = Time.time;
            rb.position = new Vector2(leftConstraint - buffer, rb.position.y);
            //tr.enabled = true;
        }
        //tr.emitting = true;




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
        startTime = Time.time;
        contact = false;
        isPressed = false;
        timeManager.SpeedUpTime();
        NormalizeTouchBaseDistance();
        //startVelocity = direction * -hopModifier * touchBaseDistance;
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
        touchBaseDistance = Mathf.Pow(Mathf.Pow((touchposition.y - basepointposition.y), 2) + Mathf.Pow((touchposition.x - basepointposition.x), 2), 0.5f);
        NormalizeTouchBaseDistance();
        setTrajectoryPoints(rb.position, direction * -hopModifier * touchBaseDistance, 0.02f);
        //trajectoryLine();
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        else if (collision.gameObject.tag == "win")
        {
            levelManagerScript.WriteLevelNum();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);            
            
        }
    }

    void NormalizeTouchBaseDistance()
    {
        if (touchBaseDistance > 5)
        {
            touchBaseDistance = 5;
        }
        else if (touchBaseDistance < 2)
        {
            touchBaseDistance = 2;
        }
        else
        {
            touchBaseDistance = Mathf.Round(touchBaseDistance * 2f) / 2f;
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
    
    void LavaAdjust()
    {
        if (transform.position.y - (lava.transform.position.y + lava.transform.localScale.y / 2) > 10)
        {
            Vector3 desiredPosition = new Vector3(0, transform.position.y - (10 + lavaScript.transform.localScale.y / 2), 1);
            Vector3 smoothedPosition = Vector3.Lerp(lava.transform.position, desiredPosition, 0.05f);
            lava.transform.position = smoothedPosition;
        }
    }



    void setTrajectoryPoints(Vector3 pStartPosition, Vector3 pVelocity, float timeStep)
    {
        float gravityScale = rb.gravityScale;
        if (rb.gravityScale < 1f)
        {
            gravityScale = 2f;
        }
        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        //Debug.Log(velocity);
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;

        fTime += timeStep;
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * gravityScale * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(pStartPosition.x + dx, pStartPosition.y + dy);
            CorrectObjectPos(ref pos.x);
            trajectoryPoints[i].transform.position = pos;
            trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
            //trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude * gravityScale) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += timeStep;
        }
    }
    void CorrectObjectPos(ref float x)
    {
        while (Mathf.Abs(x) > rightConstraint)
        {
            if (x > rightConstraint)
            {
                x -= rightConstraint * 2f;
            }
            else if (x < leftConstraint)
            {
                x += rightConstraint * 2f;
            }
        }
    }

    void UpdatePlayerPos()
    {
        
        //calculate pos with function in relation to start time & start position
        float velocity = Mathf.Sqrt((startVelocity.x * startVelocity.x) + (startVelocity.y * startVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(startVelocity.y, startVelocity.x));
        float timeElapsed = Time.time - startTime;
        float dx = velocity * timeElapsed * Mathf.Cos(angle * Mathf.Deg2Rad);
        float dy = velocity * timeElapsed * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * rb.gravityScale * timeElapsed * timeElapsed / 2.0f);
        Vector2 pos = new Vector2(startPos.x + dx, startPos.y + dy);
        CorrectObjectPos(ref pos.x);
        transform.position = pos;
        
    
    }

    IEnumerator ResetTrailRenderer()
    {
        float trailTime = tr.time;
        tr.time = 0;
        yield return null;
        tr.time = trailTime;
    }

}