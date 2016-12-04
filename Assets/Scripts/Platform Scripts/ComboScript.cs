using UnityEngine;
using System.Collections;

//Script to handle special conditions for a combo platform
public class ComboScript : PlatformScript
{
    public int ComboAdditionsPerHit;
    public int MaxHits;
    private int hitCounter = 0;

    /// <summary>
    /// if player collides with this sprite, add to their combo. There is a maximum number of times this platform can be used(cannot be spammed)
    /// </summary>
    /// <param name="player">The player gameobject</param>
    /// <param name="hitNormal">The normal of the collision</param>
    /// <param name="hitVelocity">The velocity the player hit the surface at</param>
    public override void OnPlayerCollide(GameObject player, Vector2 hitNormal, Vector2 hitVelocity)
    {
        if (!shouldApplyEffect(hitNormal) || hitCounter >= MaxHits)
        {
            return;
        }

        for (int a = 0; a < ComboAdditionsPerHit; a++)
        {
            player.GetComponent<GameHandler>().UpdateCombo(false);
        }
        hitCounter++;

        if(hitCounter >= MaxHits)
        {
            GetComponent<SpriteRenderer>().sprite = disabledSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
