using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {

    public float MinSpeed;
    public float MaxSpeed;

    public float SpeedInterval;
    public float SpeedIntervalRate = 1.0f;

    private float score;

    public float GetScore()
    {
        return score;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
