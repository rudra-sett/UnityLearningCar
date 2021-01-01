using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rigid;
    public bool dead;

    private bool gofront;
    private bool goback;
    private bool turnleft;
    private bool turnright;

    void Start()
    {
        //goforward();
        //steerright();
    }

    // Update is called once per frame
    void Update()
    {
        //frontdist = Physics.Raycast(fray, 100, out hitinfo);
        if (Input.GetKey("w"))
        {
            gofront = true;
        }
      
        if (Input.GetKey("s"))
        {
            goback = true;
        }
        if (Input.GetKey("a"))
        {
            turnleft = true;
        }
        if (Input.GetKey("d"))
        {
            turnright = true;
        }

        if (gofront)
        {
            //rigid.velocity = rigid.velocity + Vector3.forward;
            //rigid.AddRelativeForce(Vector3.up * 20);
            rigid.AddRelativeForce(Vector3.forward * 30);
        }
        if (goback)
        {
            //rigid.velocity = rigid.velocity + Vector3.back;
            //rigid.AddRelativeForce(Vector3.up * -20);
            rigid.AddRelativeForce(Vector3.forward * -30);
        }

        if (turnleft)
        {
            //rigid.angularVelocity = rigid.angularVelocity - transform.up; 
            rigid.AddRelativeTorque(-Vector3.up*20);
        }
        if (turnright)
        {
            //rigid.angularVelocity = rigid.angularVelocity + transform.up;
            rigid.AddRelativeTorque(Vector3.up*20);
        }
        //rigid.velocity *= 0.98f;
        //rigid.angularVelocity *= 0.95f;
        gofront = false;
        goback = false;
        turnleft = false;
        turnright = false;
        

    }
    public void goforward()
    {
        gofront = true;
        //rigid.velocity = rigid.velocity + Vector3.forward;
        //rigid.AddForce(Vector3.forward*20);
    }
    public void gobackward()
    {
        goback = true;
        //rigid.velocity = rigid.velocity + Vector3.forward;
       // rigid.AddForce(Vector3.forward * -20);
    }
    public void steerright()
    {
        turnright = true;
        //rigid.AddTorque(transform.forward *20,ForceMode.Impulse);
      //  rigid.AddTorque(transform.forward *-20, ForceMode.Acceleration);
    }
    public void steerleft()
    {
        turnleft = true;
      //  rigid.AddTorque(transform.forward * -20);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Floor" & collision.gameObject.tag !="Player")
        {
            dead = true;
        }
    }

}

