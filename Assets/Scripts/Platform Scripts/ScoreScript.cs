using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : PlatformScript
{
    public int MaxHits;
    //min > 1, max <= 2
    public float ScoreMulOnHit = 1.2f;
    private int hitCounter = 0;

    public override void OnPlayerCollide(GameObject player, Vector2 hitNormal, Vector2 hitVelocity)
    {
        if(!shouldApplyEffect(hitNormal) || MaxHits <= hitCounter)
        {
            return;
        }

        player.GetComponent<GameHandler>().MulScore(ScoreMulOnHit);

        hitCounter++;

        if (hitCounter >= MaxHits)
        {
            GetComponent<SpriteRenderer>().sprite = disabledSprite;
        }
    }


    // Update is called once per frame
    void Update ()
    {
	}
}
