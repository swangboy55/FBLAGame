using UnityEngine;
using System.Collections;

//script to cycle through hues for the color of a text object
public class VictoryColorChanger : MonoBehaviour
{
    private float currentHue;

	// Use this for initialization
    //set the starting hue of the text to red
	void Start () {
        currentHue = 0;
	}

    /// <summary>
    /// on every update, incriment hue ever so slightly and change the text color component of the text object this script
    /// is attatched to.
    /// </summary>
    void Update()
    {
        
        if (currentHue > 1)
        {
            currentHue = 0;
        }

        currentHue = currentHue + 0.001f;
        gameObject.GetComponent<UnityEngine.UI.Text>().color = Color.HSVToRGB(currentHue, 0.7294f, 1);
    }
}
