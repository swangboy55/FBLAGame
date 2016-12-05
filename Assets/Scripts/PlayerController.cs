using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The class to handle the movement of the player. This class is complicated, as getting the player movement down is arguably the 
/// most important thing when it comes to making this game. It is based almost entirely on how the player moves afterall.
/// </summary>
public class PlayerController : MonoBehaviour {

    public GameObject impactSound;
    public float Acceleration;
    public float MaxSpeed;
    public float JumpImpulse;
    public float JumpImpulseWJ;
    public float VelocityInvertTime = 0.5f;
    private float jumpTime = -55;
    private float collisionTime;
    private Vector2 collisionNormal;
    private Vector2 hitVelocity;
    private Vector2 velocityPrevTick;
    private bool colliding;
    private bool spaceHeld = false;
    
    //necessary input data
    private bool jumpDown = false;
    private float vAxis = 0;
    private float hAxis = 0;

    void Start()
    {
    }

    /// <summary>
    /// this function determines whether or not we have a collision, what the hitvelocity, and what the hitnormal are
    /// </summary>
    /// <param name="colliders"></param>
    /// <param name="rigid"></param>
    void UpdateCollisionData(Collider2D[] colliders, Rigidbody2D rigid)
    {
        //if there were no collisions
        if(colliders.Length == 0)
        {
            colliding = false;
        }
        else //otherwise our overlap test found something
        {
            //get some collision info from our colliders (just the hit normal)
            //as the array of Collider2D's do not provide this
            RaycastHit2D[][] hits2D = new RaycastHit2D[colliders.Length][];
            int inc = 0;
            int size = 0;
            PlatformScript script = null;
            foreach (Collider2D collider in colliders)
            {
                //our platform script assuming the platform we hit has one
                script = collider.gameObject.GetComponent<PlatformScript>();
                //cast a line to get hit normals
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
            //if we weren't colliding before, but we are now
            if (!colliding)
            {
                //play the impact sound if we just hit something
                impactSound.GetComponent<AudioSource>().Play();
                colliding = true;
                collisionTime = Time.time;
                int bestChoice = 0;
                //find the collision normal that is facing upwards/downwards the most
                //essentially find the most optimal surface we are hitting, prefer hits facing upwards/downwards
                Vector2 nearestVerticalNormal = hits[0].normal;
                for (int a = 1; a < hits.Length; a++)
                {
                    if (Mathf.Abs(nearestVerticalNormal.y) < Mathf.Abs(hits[a].normal.y))
                    {
                        bestChoice = a;
                        nearestVerticalNormal = hits[a].normal;
                    }
                }
                collisionNormal = nearestVerticalNormal;
                script = hits[bestChoice].collider.gameObject.GetComponent<PlatformScript>();
                //if there is a script
                if (script != null)
                {
                    //run the script
                    script.OnPlayerCollide(gameObject, collisionNormal, rigid.velocity);
                }
                //set our hitvelocity to our velocity from the previous update
                hitVelocity = velocityPrevTick;
            }
            else//otherwise we already are colliding with something
            {
                int bestChoice = 0;
                //we need to check if we're hitting something new
                Vector2 nearestVerticalNormal = hits[0].normal;
                for(int a=1;a<hits.Length;a++)
                {
                    if(Mathf.Abs(nearestVerticalNormal.y) < Mathf.Abs(hits[a].normal.y))
                    {
                        bestChoice = a;
                        nearestVerticalNormal = hits[a].normal;
                    }
                }

                //if we are hitting something new
                if(collisionNormal.y != nearestVerticalNormal.y || collisionNormal.x != nearestVerticalNormal.x)
                {
                    //play an impact sound
                    impactSound.GetComponent<AudioSource>().Play();
                    collisionNormal = nearestVerticalNormal;
                    hitVelocity = velocityPrevTick;
                    collisionTime = Time.time;
                    script = hits[bestChoice].collider.gameObject.GetComponent<PlatformScript>();
                    if (script != null)
                    {
                        //run script
                        script.OnPlayerCollide(gameObject, collisionNormal, rigid.velocity);
                    }
                }
            }
        }

    }

    /// <summary>
    /// manages input from the player, updates player velocity vector based on input
    /// </summary>
	void Update()
    {
        //check if first press of space
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

        //back to menu on escape
        if(Input.GetKey(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            GameHandler.lives = 3;
        }

        //get horizontal input (left/right arrow and A/D)
        hAxis = Input.GetAxis("Horizontal");

        //get vertical input (down arrow / s and spacebar)
        vAxis = ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) ? -1 : (Input.GetKey(KeyCode.Space) ? 1 : 0));
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        Vector2 newVelocity;
        //check collisions with an enlarged circle of the player, ignore player collision itself with layermask
        Collider2D[] coll = Physics2D.OverlapCircleAll(rigid.position, GetComponent<CircleCollider2D>().radius * 2f, 1 << LayerMask.NameToLayer("Default"));

        //update collision data based on overlap results
        UpdateCollisionData(coll, rigid);
        
        //if we are in the air
        if (!colliding)
        {
            //velocity is damped vertically, and accelerated horizontally based on player input
            newVelocity = rigid.velocity + new Vector2(hAxis * Acceleration * Time.deltaTime,
                vAxis * rigid.gravityScale * (-Physics2D.gravity.y / 2.0f) * Time.deltaTime);
        }
        else //otherwise we are colliding with something
        {
            //if we have been on the ground past the special jump time, reset the combo
            if(collisionTime + VelocityInvertTime < Time.time)
            {
                GetComponent<GameHandler>().UpdateCombo(true);
            }
            //set the horizontal velocity based on player input
            newVelocity = rigid.velocity + new Vector2(hAxis * Acceleration * Time.fixedDeltaTime, 0);
            //if this is the first frame we pressed the jump key
            if (jumpDown)
            {
                //add 1 to combo and update the score
                GetComponent<GameHandler>().UpdateCombo(false);
                GetComponent<GameHandler>().UpdateScore(hitVelocity.magnitude);
                //save the time when we jumped
                jumpTime = Time.time;

                //if we jumped after the special jump time window is over
                if (collisionTime + VelocityInvertTime < Time.time)
                {
                    //do a normal jump off the surface using the jumpimpulse
                    newVelocity += (collisionNormal.normalized * JumpImpulse);
                }
                else //otherwise we jumped in time to get a special jump
                {
                    //scalingfactor represents a factor to scale inhereted x velocity by, but only in the case that the intended direction of the player is opposite to the vector that is created on the collision.
                    float scalingfactor = 0.3f;
                    float minWallLaunch = 10.0f;

                    //get the x direction of our movement pre-collision
                    float moveSign = Mathf.Sign(-hitVelocity.x);
                    
                    //If statement checks if the player is intending to wall-jump into the wall, or jump away from it. Vectors for each are significantly different.
                    if (Mathf.Sign(hAxis) == /*moveSign*/Mathf.Sign(collisionNormal.x) || collisionNormal.x == 0)
                    {
                        //our new velocity is the result of a dot product of a new hitnormal of (x, 1-y)
                        //this new normal is to handle the fact that if a surface is horizontal, we shouldn't be launched really far up if our Y vel is high,
                        //but if the surface is vertical then we want to go really fast left/right
                        newVelocity += (Mathf.Abs(hitVelocity.x * collisionNormal.x) + Mathf.Abs(hitVelocity.y * (1 - collisionNormal.y))) * collisionNormal;

                        //if the normal is NOT facing down, we add a jump impulse
                        if(Mathf.Abs(collisionNormal.y) <= 0.0001f || collisionNormal.y >= 0)
                        {
                            newVelocity += new Vector2(0, JumpImpulse);
                        }
                    }
                    else //this is the case where we are intending to wall jump
                    {
                        //the result is an impulse going much higher up, and it doesn't launch you away from the wall as fast
                        //this is intended to help you be able to wall jump again before you lose too much y velocity
                        newVelocity += (Vector2.Dot(new Vector2(moveSign * Mathf.Max(Mathf.Abs(-hitVelocity.x * scalingfactor), minWallLaunch), 0), collisionNormal) * collisionNormal) + new Vector2(0, JumpImpulseWJ);
                    }
                }
            }
        }
        velocityPrevTick = newVelocity;
        //set our new velocity
        GetComponent<Rigidbody2D>().velocity = newVelocity;
        //update our combo animation
        GetComponent<GameHandler>().UpdateComboPretty(Time.time - jumpTime, VelocityInvertTime);
    }
}
