using UnityEngine;
using System.Collections;

//Script to handle special conditions for boost platform
public class BoostScript : PlatformScript {

    public float BoostFactor = 3;

    /// <summary>
    /// On player collide, reflect player velocity across the normal and multiply it.
    /// </summary>
    /// <param name="player">The player gameobject</param>
    /// <param name="hitNormal">The normal of the collision</param>
    /// <param name="hitVelocity">The velocity the player hit the surface at</param>
    public override void OnPlayerCollide(GameObject player, Vector2 hitNormal, Vector2 hitVelocity)
    {
        if (!shouldApplyEffect(hitNormal))
        {
            return;
        }
        Vector2 velAdd = hitVelocity.magnitude * hitNormal * BoostFactor;

        if(hitNormal.y < -0.001f)
        {
            velAdd += new Vector2(0, -(player.GetComponent<PlayerController>().JumpImpulse));
        }
        else
        {
            velAdd += new Vector2(0, player.GetComponent<PlayerController>().JumpImpulse);
        }
        Vector2 v = player.GetComponent<Rigidbody2D>().velocity;
        Vector2 phn = new Vector2(hitNormal.y, -hitNormal.x);
        v = (Vector2.Dot(v, phn) * phn);
        player.GetComponent<Rigidbody2D>().velocity = (v + velAdd);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
