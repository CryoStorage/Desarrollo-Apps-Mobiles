using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //the vector that the player moves towards
    private Vector3 dir = new Vector3(0,0,0);
    private Vector3 jumpDir = new Vector3 (1,1,0);
    private float jumpForce = 6F;
    private float maxJump = 12f;
    //boolean used for subtracting the Y component of dir from itself for only 1 frame
    bool cancelY = false;
    private Vector3 forceAdded = new Vector3(0,0,0);
    private float speed = .3f;
    private float maxSpeed = .50f;
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
        if (cancelY == true);
        {
            Vector3 temp = new Vector3(0,dir.y,0);
            dir =- temp;
            cancelY = false;
        }


    }

    void CheckInput()
    {
        float mousePos = Input.mousePosition.x;
        ChargeJump(0);

    }

    void ChargeJump(int direction)
    {
        if (Input.GetMouseButtonDown(0))
        {
            dir = Vector3.zero;
            Debug.Log("pressed m1");

        }
        if (Input.GetMouseButton(0))
        {
            forceAdded += jumpDir*jumpForce * Time.fixedDeltaTime;
            Debug.Log("holding m1");

        }
        if (Input.GetMouseButtonUp(0))
        {
            Jump(forceAdded);
            Debug.Log("released m2");
            
        }
    }

    void Jump(Vector3 force)
    {
        if (force.magnitude >= maxJump)
        {

            dir += force.normalized* maxJump * Time.fixedDeltaTime;
        }else
        {

            dir += force*Time.fixedDeltaTime;
            
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
