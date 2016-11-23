using UnityEngine;
using System.Collections;

public class BallColorChanger : MonoBehaviour
{

    public float HueIncrement;
    private float currentHue;

	// Use this for initialization
	void Start () {
        currentHue = 0;
	}
	
    void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(currentHue, 1, 1);
        currentHue += HueIncrement;
        if (currentHue > 1)
        {
            currentHue -= (int)currentHue;
        }
    }
}
