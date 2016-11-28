using UnityEngine;
using System.Collections;

public class BallColorChanger : MonoBehaviour
{
    public float VelocityUpperBound;
    private float currentHue;

	// Use this for initialization
	void Start () {
        currentHue = 0.8f;
	}

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
