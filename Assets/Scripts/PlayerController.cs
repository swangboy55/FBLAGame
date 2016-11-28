using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public float Acceleration;
    public float MaxSpeed;
    public float JumpImpulse;
    public float VelocityInvertTime = 0.5f;
    private float collisionTime;
    private Vector2 collisionNormal;
    private Vector2 hitVelocity;
    private bool colliding = false;
    private bool spaceHeld = false;

    private float gravSave;

    //necessary input data
    private bool jumpDown = false;
    private bool spacePressed = false;
    private float vAxis = 0;
    private float hAxis = 0;

    void Start()
    {
        gravSave = GetComponent<Rigidbody2D>().gravityScale;
    }

    void FixedUpdate()
    {
        
    }

	void Update()
    {
        if(Input.GetKey(KeyCode.Space) && !spaceHeld)
        {
            jumpDown = true;
            spaceHeld = true;
        }
        else if(!Input.GetKey(KeyCode.Space))
        {
            spaceHeld = false;
            jumpDown = false;
        }
        else
        {
            jumpDown = false;
        }

        hAxis = Input.GetAxis("Horizontal");

        vAxis = (Input.GetKey(KeyCode.S) ? -1 : (Input.GetKey(KeyCode.Space) ? 1 : 0));
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        Vector2 newVelocity;
        if (!colliding)
        {
            newVelocity = rigid.velocity + new Vector2(hAxis * Acceleration * Time.fixedDeltaTime,
                vAxis * rigid.gravityScale * (-Physics2D.gravity.y / 2.0f) * Time.fixedDeltaTime);
        }
        else
        {
            newVelocity = rigid.velocity + new Vector2(hAxis * Acceleration * Time.fixedDeltaTime, 0);
            if (jumpDown)
            {
                rigid.gravityScale = gravSave;
                if (collisionTime + VelocityInvertTime < Time.time)
                {
                    newVelocity += (collisionNormal.normalized * JumpImpulse);
                }
                else
                {
                    newVelocity += (Vector2.Dot(new Vector2(-hitVelocity.x, 0), collisionNormal) * collisionNormal) + new Vector2(0, JumpImpulse);
                    //Debug.Log(newVelocity);
                }
            }
        }

        if (collisionTime + VelocityInvertTime < Time.time)
        {
            Debug.Log("VI");
        }
        else
        {
            Debug.Log("NO VI");
        }

        GetComponent<Rigidbody2D>().velocity = newVelocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        colliding = true;
        collisionNormal = col.contacts[0].normal;
        hitVelocity = col.relativeVelocity;
        collisionTime = Time.time;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        foreach(ContactPoint2D contact in col.contacts)
        {
            if(contact.normal.x == collisionNormal.x && contact.normal.y == collisionNormal.y)
            {
                colliding = false;
                break;
            }
        }
    }
}
