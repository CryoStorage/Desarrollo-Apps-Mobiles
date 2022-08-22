using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private Vector3 dir = new Vector3(0,0,0);
    [SerializeField] float speed = 1;
    [SerializeField] float maxSpeed;
    [SerializeField] Vector3 g = new Vector3(0,1f,0);

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
            dir += ((g*speed) * (Time.fixedDeltaTime));
        }

    }

    void CheckInput()
    {      
        if(Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x <= Screen.width / 2)
            {
                Debug.Log("L");
                Jump(0);
                
            }else
            {
                Debug.Log("R");
                Jump(1);
            }
            //Jump();
        }
    }

    void Jump(int dir)
    {
        

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
