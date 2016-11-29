using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public float Acceleration;
    public float MaxSpeed;
    public float JumpImpulse;
    public float VelocityInvertTime = 0.5f;
    private float collisionTime;
    private List<Vector2> collisionNormal = new List<Vector2>();
    private List<Vector2> hitVelocity = new List<Vector2>();
    private bool colliding { get { return collisionNormal.Count > 0; } }
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
        if(jumpDown)
        {
            int a = 0;
        }
        if (!colliding)
        {
            newVelocity = rigid.velocity + new Vector2(hAxis * Acceleration * Time.deltaTime,
                vAxis * rigid.gravityScale * (-Physics2D.gravity.y / 2.0f) * Time.deltaTime);
        }
        else
        {
            newVelocity = rigid.velocity + new Vector2(hAxis * Acceleration * Time.fixedDeltaTime, 0);
            if (jumpDown)
            {
                rigid.gravityScale = gravSave;
                if (collisionTime + VelocityInvertTime < Time.time)
                {
                    newVelocity += (collisionNormal[0].normalized * JumpImpulse);
                }
                else
                {
                    newVelocity += (Vector2.Dot(new Vector2(-hitVelocity[0].x, 0), collisionNormal[0]) * collisionNormal[0]) + new Vector2(0, JumpImpulse);
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
        collisionNormal.Add(col.contacts[0].normal);
        hitVelocity.Add(col.relativeVelocity);
        collisionTime = Time.time;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        foreach(ContactPoint2D contact in col.contacts)
        {
            for (int a = 0; a < collisionNormal.Count; a++)
            {
                if (contact.normal.x == collisionNormal[a].x && contact.normal.y == collisionNormal[a].y)
                {
                    collisionNormal.RemoveAt(a);
                    hitVelocity.RemoveAt(a);
                    break;
                }
            }
        }
    }
}
