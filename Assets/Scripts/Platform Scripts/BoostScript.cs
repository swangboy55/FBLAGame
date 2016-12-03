using UnityEngine;
using System.Collections;

public class BoostScript : PlatformScript {

    public float BoostFactor = 3;

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
        player.GetComponent<Rigidbody2D>().velocity += velAdd;
        //Debug.Log(player.GetComponent<Rigidbody2D>().velocity);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
