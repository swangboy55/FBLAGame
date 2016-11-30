﻿using UnityEngine;
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
    private bool colliding;
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

    void UpdateCollisionData(Collider2D[] colliders, Rigidbody2D rigid)
    {
        if(colliders.Length == 0)
        {
            colliding = false;
        }
        else
        {

            RaycastHit2D[][] hits2D = new RaycastHit2D[colliders.Length][];
            int inc = 0;
            int size = 0;
            foreach (Collider2D collider in colliders)
            {
                hits2D[inc] = Physics2D.LinecastAll(rigid.position, collider.transform.position, 1 << LayerMask.NameToLayer("Default"));
                size += hits2D[inc].Length;
                inc++;
            }

            RaycastHit2D[] hits = new RaycastHit2D[size];
            inc = 0;
            for (int a = 0;a<hits2D.Length;a++)
            {
                for(int b=0;b<hits2D[a].Length;b++)
                {
                    hits[inc] = hits2D[a][b];
                    inc++;
                }
            }
            if (!colliding)
            {
                colliding = true;
                collisionTime = Time.time;
                Vector2 nearestVerticalNormal = hits[0].normal;
                for (int a = 1; a < hits.Length; a++)
                {
                    if (Mathf.Abs(nearestVerticalNormal.y) < Mathf.Abs(hits[a].normal.y))
                    {
                        nearestVerticalNormal = hits[a].normal;
                    }
                }
                collisionNormal = nearestVerticalNormal;
                //hitVelocity = rigid.velocity;
            }
            else
            {
                Vector2 nearestVerticalNormal = hits[0].normal;
                for(int a=1;a<hits.Length;a++)
                {
                    if(Mathf.Abs(nearestVerticalNormal.y) < Mathf.Abs(hits[a].normal.y))
                    {
                        nearestVerticalNormal = hits[a].normal;
                    }
                }
                if(collisionNormal.y != nearestVerticalNormal.y || collisionNormal.x != nearestVerticalNormal.x)
                {
                    collisionNormal = nearestVerticalNormal;
                    //hitVelocity = rigid.velocity;
                    collisionTime = Time.time;
                }
            }
        }

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
        Collider2D[] coll = Physics2D.OverlapCircleAll(rigid.position, GetComponent<CircleCollider2D>().radius * 1.5f, 1 << LayerMask.NameToLayer("Default"));

        UpdateCollisionData(coll, rigid);

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
                    newVelocity += (collisionNormal.normalized * JumpImpulse);
                }
                else
                {
                    newVelocity += (Vector2.Dot(new Vector2(-hitVelocity.x, 0), collisionNormal) * collisionNormal) + new Vector2(0, JumpImpulse);
                    //Debug.Log((Vector2.Dot(new Vector2(hitVelocity.x, 0), collisionNormal) * collisionNormal));
                }
            }
        }
        GetComponent<Rigidbody2D>().velocity = newVelocity;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        hitVelocity = col.relativeVelocity;
    }

    //void OnCollisionExit2D(Collision2D col)
    //{
    //    foreach(ContactPoint2D contact in col.contacts)
    //    {
    //        for (int a = 0; a < collisionNormal.Count; a++)
    //        {
    //            if (contact.normal.x == collisionNormal[a].x && contact.normal.y == collisionNormal[a].y)
    //            {
    //                collisionNormal.RemoveAt(a);
    //                hitVelocity.RemoveAt(a);
    //                break;
    //            }
    //        }
    //    }
    //}
}