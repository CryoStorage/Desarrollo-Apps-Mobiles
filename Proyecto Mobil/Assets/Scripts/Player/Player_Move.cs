using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //the vector that the player moves towards
    private Vector3 dir = new Vector3(0,0,0);
    private Vector3 jumpDir = new Vector3 (1,1,0);
    private float jumpForce = 6F;
    private Vector3 maxJump = new Vector3(12f,12,0);
    //boolean used for subtracting the Y component of dir from itself for only 1 frame
    private Vector3 forceAdded = new Vector3(0,0,0);
    private float speed = .3f;
    private float maxSpeed = .50f;
    private float friction = .50f;
    private Vector3 g = new Vector3(0,1f,0);

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

    }
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        Mathf.Clamp(dir.magnitude,-maxSpeed,maxSpeed);
        Gravity();
        con.Move(dir);
    }
    void Gravity()
    {
        if (! con.isGrounded)
        {
            dir -= ((g*speed) * (Time.fixedDeltaTime));
        }
        if (con.isGrounded)
        {
            Vector3 frictionVector = new Vector3(friction,0,0);
            if (dir.x != 0)
            {
                if (dir.x > 0)
                {
                    dir -= frictionVector*Time.fixedDeltaTime;
                }
                if(dir.x < 0)
                {
                    dir += frictionVector*Time.fixedDeltaTime;
                }
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
        
        if (Input.GetMouseButtonDown(0) && con.isGrounded)
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
                Jump(forceAdded,0);
                forceAdded = Vector3.zero;
                //Debug.Log("Jump-Left");
            }else
            {
                Jump(forceAdded,1);
                forceAdded = Vector3.zero;
                //Debug.Log("Jump-Right");
            }
        }
    }
    void Jump(Vector3 force, float direction)
    {
        if(direction == 1)
        {
            if(dir.y > 0)
            {
                Vector3 cancelY = new Vector3(0,dir.y,0);
                dir += (force + cancelY) * Time.fixedDeltaTime;
            }else
            {
                dir += force*Time.fixedDeltaTime;
            }
        }
        if (direction == 0)
        {
            //Creating new vector to invert force in x axis
            Vector3 temp = new Vector3 (-force.x,force.y,force.z);
            if(dir.y > 0)
            {
                Vector3 cancelY = new Vector3(0,dir.y,0);
                dir += (force + cancelY) * Time.fixedDeltaTime;
            }else
            {
                dir += temp*Time.fixedDeltaTime;
            }
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