using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class PlatformScript : MonoBehaviour
{
    protected Vector2 normalCompare;
    protected Sprite disabledSprite;

    /// <summary>
    /// Start function for script, calculate the -approximate- direction of the top of the surface
    /// </summary>
    public void Start()
    {
        disabledSprite = Resources.Load<Sprite>("Sprites/spriteplatformdisabled");
        Vector3 rotation = GetComponent<Transform>().rotation.eulerAngles;
        normalCompare = (new Vector2(Mathf.Cos(Mathf.Deg2Rad * (rotation.z + 90.0f)), Mathf.Sin(Mathf.Deg2Rad * (rotation.z + 90.0f)))).normalized;
        Debug.Log(normalCompare);
    }

    /// <summary>
    /// Returns whether or not the hitNormal is approx. equal to the effect normal of the platform
    /// </summary>
    /// <param name="hitNormal">Player's hit normal</param>
    /// <returns></returns>
    protected bool shouldApplyEffect(Vector2 hitNormal)
    {
        Vector2 hnn = hitNormal.normalized;
        //large tolerance to account for float inacc in normalcompare calculation
        //as well as there being only 4 possible states for the hitNormal == normalCompare test
        return (Mathf.Abs(hnn.x - normalCompare.x) < 0.01f) && (Mathf.Abs(hnn.y - normalCompare.y) < 0.01f);
    }

    /// <summary>
    /// Abstract function, called when a player collides with a platform using this script
    /// </summary>
    /// <param name="player">The player gameobject</param>
    /// <param name="hitNormal">The normal of the collision</param>
    /// <param name="hitVelocity">The velocity the player hit the surface at</param>
    public abstract void OnPlayerCollide(GameObject player, Vector2 hitNormal, Vector2 hitVelocity);
}
