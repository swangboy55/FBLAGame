using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to handle special conditions for a score platform
public class TimerScript : PlatformScript
{
    public float BonusTime;
    public int MaxHits;
    private int hitCounter = 0;
    /// <summary>
    /// On player collision with this sprite, decrease their time below requirement by an amount.
    /// </summary>
    /// <param name="player">The player gameobject</param>
    /// <param name="hitNormal">The normal of the collision</param>
    /// <param name="hitVelocity">The velocity the player hit the surface at</param>
    public override void OnPlayerCollide(GameObject player, Vector2 hitNormal, Vector2 hitVelocity)
    {
        if(!shouldApplyEffect(hitNormal) || MaxHits <= hitCounter)
        {
            return;
        }
        player.GetComponent<GameHandler>().UpdateTimer(BonusTime);

        hitCounter++;

        if(MaxHits <= hitCounter)
        {
            GetComponent<SpriteRenderer>().sprite = disabledSprite;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
