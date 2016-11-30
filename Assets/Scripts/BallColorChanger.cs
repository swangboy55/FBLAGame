using UnityEngine;
using System.Collections;

//Script to change color of character based on their velocity.
public class BallColorChanger : MonoBehaviour
{
    public float VelocityUpperBound;
    private float currentHue;

	// Use this for initialization
    //Initial value is a hue which is similar to the desired color of the player when vel = 0
	void Start () {
        currentHue = 0.8f;
	}

    /// <summary>
    /// Check on every frame what the players velocity is, and set a given velocity to a given color.
    /// Also defines an upper bound for the color changing, such that VelocityUpperBound and any value above it keeps the color red.
    /// VelocityUpperBound therefore also defines the scaling for color changing. 
    /// </summary>
    void Update()
    {
        currentHue = (GetComponent<Rigidbody2D>().velocity.magnitude / VelocityUpperBound);
        if (currentHue > 1)
        {
            currentHue = 1;
        }
        currentHue *= 0.8f;
        currentHue = 0.8f - currentHue;
        gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(currentHue, 1, 1);
    }
}
