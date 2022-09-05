using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Move : MonoBehaviour
{
    //the vector that the player moves towards
    private Vector3 dir = new Vector3(0,0,0);
    private Vector3 jumpDir = new Vector3 (1,1,0);
    private float jumpForce = 8F;
    private Vector3 maxJump = new Vector3(12f,12,0);
    //boolean used for subtracting the Y component of dir from itself for only 1 frame
    private Vector3 forceAdded = new Vector3(0,0,0);
    private float speed = .3f;
    private float maxSpeed = .50f;
    private float friction = 10f;
    private Vector3 g = new Vector3(0,1f,0);
    private bool sticky = false;
    private bool wallcheck = true;
    private bool grounded = false;
    CharacterController con;
    // Start is called before the first frame update
    void Start()
    {
        Preprare();
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckGround();
        CheckWall();
        Stick();
        //Debug.Log("Sticky is : " + sticky);
        Debug.Log("grounded is : " + grounded);
    }
    void FixedUpdate()
    {
        Move();
    }
    void Stick()
    {
        if (sticky)
        {
            dir = Vector3.zero;
        }
    }
    void CheckGround()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0,.5f,0);
        if(Physics.Raycast(transform.position, Vector3.down,out hit, con.height/2f+.1f) && hit.collider.tag == "Floor")
        {
            grounded = true;
            CancelY();
        }else{grounded = false;}
    }
    void CancelY()
    {
        if(dir.y != 0 && grounded)
        {
            Vector3 cancelY = new Vector3(0,dir.y,0);
            dir -= cancelY * Time.fixedDeltaTime;
        }
    }
    void CheckWall()
    {
        RaycastHit hit;
        if (wallcheck)
        {
            for (int i = 0; i < 1; i++)
            {
                Physics.Raycast(transform.position, Vector3.right,out hit, con.radius+.1f);
                Debug.Log("CastingRays");
                if (hit.collider != null && hit.collider.tag == "Wall")
                {
                    sticky = true;
                }else{sticky = false;}
                
                Physics.Raycast(transform.position, -Vector3.right,out hit, con.radius+.1f);
                if (hit.collider != null && hit.collider.tag == "Wall")
                {
                    sticky = true;
                }else{sticky = false;}
                
            }
        }else
        {
            Debug.Log("Not CastingRays");
        } 

    }

    IEnumerator corWallCheck()
    {
        wallcheck = false;
        yield return new WaitForEndOfFrame();
        wallcheck = true;
    }
    void Gravity()
    {
        if (!grounded)
        {
            dir -= ((g*speed) * (Time.fixedDeltaTime));
        }
        if (grounded)
        {
            Vector3 frictionVector = new Vector3(dir.x * friction,0,0);
            if (dir.x != 0)
            {
                dir -= frictionVector*Time.fixedDeltaTime;
            }
        }
    }
    void CheckInput()
    {
        ChargeJump();
    }
    void ChargeJump()
    {
        float mousePos = Input.mousePosition.x;
        
        if (Input.GetMouseButtonDown(0) && grounded )
        {
            // removes previous momentum
            dir = Vector3.zero;
        }
        if (Input.GetMouseButton(0) && grounded)
        {
            //Limits jump force to asigned max value
            if (forceAdded.magnitude < maxJump.magnitude)
            {
                forceAdded += jumpDir * jumpForce * Time.fixedDeltaTime;   
            }
        }
        if (Input.GetMouseButtonUp(0) && grounded)
        {
            //Checks screen position of the cursor and calls jump with..
            //  it's corresponding value then resets forceAdded to zero
            if(mousePos < Screen.width/2)
            {
                Jump(forceAdded,0);
                forceAdded = Vector3.zero;
            }else
            {
                Jump(forceAdded,1);
                forceAdded = Vector3.zero;
            }
        }
    }
    void Jump(Vector3 force, float direction)
    {
        if(direction == 1)
        {
            //jumps right
            StopAllCoroutines();
            StartCoroutine(corWallCheck());
            
            dir += force*Time.fixedDeltaTime;

        }
        if (direction == 0)
        {
            //Creating new vector to invert force in x axis(jumps left)
            Vector3 temp = new Vector3 (-force.x,force.y,force.z);

            StopAllCoroutines();
            StartCoroutine(corWallCheck());
            dir += temp*Time.fixedDeltaTime;
        }
    }
    void Move()
    {
        Mathf.Clamp(dir.magnitude,-maxSpeed,maxSpeed);
        Gravity();
        con.Move(dir);

    }
    void Preprare()
    {
        Application.targetFrameRate = 60;
        if (con == null)
        {
            try
            {
                con = GetComponent<CharacterController>();
            }  catch { Debug.LogWarning("could not add CharacterController"); }
        }
    }
}