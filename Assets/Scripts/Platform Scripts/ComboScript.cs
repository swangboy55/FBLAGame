using UnityEngine;
using System.Collections;

public class ComboScript : PlatformScript
{
    public int ComboAdditionsPerHit;
    public int MaxHits;
    private int hitCounter = 0;

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
