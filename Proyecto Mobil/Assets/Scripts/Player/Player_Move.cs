using System.Collections;
using UnityEngine;
public class Player_Move : MonoBehaviour
{
    //the vector that the player moves towards
    private Vector3 dir = Vector3.zero;
    private Vector3 jumpDir = new Vector3 (1,1,0);
    [SerializeField] private float jumpForce;
    private Vector3 forceAdded = Vector3.zero;
    private Vector3 maxJump = new Vector3(12f,12,0);
    private float speed = .3f;
    private float friction = 10f;
    private Vector3 g = new Vector3(0,1f,0);
    private bool sticky;
    private bool grounded;
    private bool wallcheck = true;
    private bool canMove = true;
    CharacterController con;
    
    
    private Player_CheckPointManager checkPointManager;
    private LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        Prepare();
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckGround();
        CheckWall();
        CheckSticky();
        Move();
    }
    public void Respawn()
    {
        StopAllCoroutines();
        StartCoroutine(CorWaitForRespawn());
        
    }

    IEnumerator CorWaitForRespawn()
    {
        dir = Vector3.zero;
        canMove = false;
        if (!canMove)
        {
            con.enabled = false;
            transform.position = checkPointManager.currentCheckPoint;
            Player_Ink.ink = 100f;
        }
        yield return new WaitForEndOfFrame();
        canMove = true;
        con.enabled = true;
        StopCoroutine(CorWaitForRespawn());
    }
    void Move()
    {
        if (!canMove) return;
        Gravity();
        con.Move(dir);
    }
    void CheckSticky()
    {
        if (sticky)
        {
            dir = Vector3.zero;
        }
    }
    void CheckGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down,out hit, con.height/2f+.1f, layerMask, QueryTriggerInteraction.Ignore) && (hit.collider.CompareTag("Floor")))
        {
            grounded = true;
            CancelY();
        }else
        {
            grounded = false;
        }
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
        if (!wallcheck) return;
        for (int i = 0; i < 1; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, -Vector3.right,out hit, con.radius+.1f);
            if (hit.collider && hit.collider.CompareTag("Wall"))
            {
                sticky = true;
            }
            else
            {
                sticky = false;
            }
            Physics.Raycast(transform.position, Vector3.right,out hit, con.radius+.1f);
            if (hit.collider && hit.collider.CompareTag("Wall"))
            {
                sticky = true;
            }
        }
    }
    private IEnumerator CorUnstickAndGround()
    {
        //unsticks the player and sets grounded to true...
        //during the frame when jump is called. then returns them to normal
        sticky = false;
        wallcheck = false;
        grounded = true;
        yield return new WaitForEndOfFrame();
        wallcheck = true;
    }
    void Gravity()
    {
        if (!grounded)
        {
            dir -= (g * (Time.fixedDeltaTime * speed));
        }
        if (!grounded) return;
        Vector3 frictionVector = new Vector3(dir.x * friction,0,0);
        if (dir.x != 0)
        {
            dir -= frictionVector*Time.fixedDeltaTime;
        }
    }
    void CheckInput()
    {
        ChargeJump();
    }
    void ChargeJump()
    {
        float mousePos = Input.mousePosition.x;
        if (Input.GetMouseButtonDown(0))
        {
            if (!grounded && !sticky) return;
            dir = Vector3.zero;
        }
        if (Input.GetMouseButton(0))
        {
            if (!grounded && !sticky) return;
            // Limits jump force to assigned max value
             if (forceAdded.magnitude < maxJump.magnitude)
             {
                 forceAdded += jumpDir * (jumpForce * Time.fixedDeltaTime);   
             }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!grounded && !sticky) return;
            //Checks screen position of the cursor and calls jump with..
            //  it's corresponding value then resets forceAdded to zero
            switch (mousePos)
            {
                case float n when( n < Screen.width/2f):
                    Jump(forceAdded,0);
                    forceAdded = Vector3.zero;
                    break;
                case float n when( n > Screen.width/2f):
                    Jump(forceAdded,1);
                    forceAdded = Vector3.zero;
                    break;
            }
        }
    }
    void Jump(Vector3 force, int direction)
    {
        if(direction == 1)
        {
            //jumps right
            StopAllCoroutines();
            StartCoroutine(CorUnstickAndGround());
            dir += force*Time.fixedDeltaTime;
        }
        if (direction == 0)
        {
            //Creating new vector to invert force in x axis(jumps left)
            Vector3 temp = new Vector3 (-force.x,force.y,force.z);
            StopAllCoroutines();
            StartCoroutine(CorUnstickAndGround());
            dir += temp*Time.fixedDeltaTime;
        }
    }
    void Prepare()
    {
        Application.targetFrameRate = 60;
        layerMask =~ LayerMask.GetMask("Player");
        if (con != null) return;
        try
        {
            con = GetComponent<CharacterController>();
        }  catch { Debug.LogWarning("could not get CharacterController"); }
        
        if (checkPointManager != null) return;
        try
        {
            checkPointManager = GetComponent<Player_CheckPointManager>();
        }
        catch{ Debug.LogWarning("Could not find Player_SpawnDie");}
    }
}