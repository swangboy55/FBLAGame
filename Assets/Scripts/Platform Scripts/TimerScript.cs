using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : PlatformScript
{
    public float BonusTime;
    public int MaxHits;
    private int hitCounter = 0;

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
