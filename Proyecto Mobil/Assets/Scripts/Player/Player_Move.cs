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
    }
    void FixedUpdate()
    {
        Move();
    }

    void Stick()
    {
        sticky = true;
        if (sticky)
        {
            Debug.Log("stick");
            dir = Vector3.zero;
            
        }

    }
    void CheckGround()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0,.5f,0);
        if(Physics.Raycast(transform.position, Vector3.down,out hit, con.height/2f+.1f) && hit.collider.tag == "Floor")
        {
            CancelY();

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
                if (hit.collider != null && hit.collider.tag == "Wall")
                {
                    Stick();
                }
                Physics.Raycast(transform.position, -Vector3.right,out hit, con.radius+.1f);
                if (hit.collider != null && hit.collider.tag == "Wall")
                {
                    Stick();
                }
            }
        }


    }
    void Move()
    {
        Mathf.Clamp(dir.magnitude,-maxSpeed,maxSpeed);
        Gravity();
        con.Move(dir);
    }
    void Gravity()
    {
        if (! con.isGrounded | sticky)
        {
            dir -= ((g*speed) * (Time.fixedDeltaTime));
        }
        if (con.isGrounded)
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
        
        if (Input.GetMouseButtonDown(0) && con.isGrounded )
        {
            dir = Vector3.zero;
        }
        if (Input.GetMouseButton(0) && con.isGrounded)
        {
            if (forceAdded.magnitude < maxJump.magnitude)
            {
                forceAdded += jumpDir * jumpForce * Time.fixedDeltaTime;   
            }
        }
        if (Input.GetMouseButtonUp(0) && con.isGrounded)
        {
            if(mousePos < Screen.width/2)
            {
                StopAllCoroutines();
                wallcheck = false;
                sticky = false;
                Jump(forceAdded,0);
                forceAdded = Vector3.zero;
                StartCoroutine(corCheckWall());
                //Debug.Log("Jump-Left");
            }else
            {
                StopAllCoroutines();
                wallcheck = false;
                sticky = false;
                Jump(forceAdded,1);
                forceAdded = Vector3.zero;
                StartCoroutine(corCheckWall());
                //Debug.Log("Jump-Right");
            }
        }
    }

    IEnumerator corCheckWall()
    {
        wallcheck = true;
        yield return new WaitForEndOfFrame();
        Debug.Log("EndOfFrame");

    }
    
    void CancelY()
    {
        if(dir.y != 0 && con.isGrounded)
        {

            Vector3 cancelY = new Vector3(0,dir.y,0);
            dir -= cancelY * Time.fixedDeltaTime;
        }
    }
    void Jump(Vector3 force, float direction)
    {
        if(direction == 1)
        {
            dir += force*Time.fixedDeltaTime;
        }
        if (direction == 0)
        {
            //Creating new vector to invert force in x axis
            Vector3 temp = new Vector3 (-force.x,force.y,force.z);
            dir += temp*Time.fixedDeltaTime;
        }
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