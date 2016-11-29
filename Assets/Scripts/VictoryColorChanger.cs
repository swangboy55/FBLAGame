using UnityEngine;
using System.Collections;

public class VictoryColorChanger : MonoBehaviour
{
    private float currentHue;

	// Use this for initialization
	void Start () {
        currentHue = 0;
	}

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
