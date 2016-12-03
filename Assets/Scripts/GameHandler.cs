using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameHandler : MonoBehaviour {

    public float MinSpeed;
    public float MaxSpeed;
    public float PreparationSeconds = 10.0f;
    public float timeAllowedUnderReq = 5f;
    public string nextScene;

    public float SpeedInterval;
    public float SpeedIntervalRate = 1.0f;

    private float score;
    private float timeStart;
    private float velocityNeeded;
    private float timeUnderReq = 0;
    private float deltaTimeSum = 0;

    public float GetScore()
    {
        return score;
    }

	// Use this for initialization
	void Start ()
    {
        timeStart = Time.time;
        velocityNeeded = MinSpeed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time < timeStart + PreparationSeconds)
        {

        }
        else
        {
            deltaTimeSum += Time.deltaTime;
            if(deltaTimeSum >= SpeedIntervalRate)
            {
                deltaTimeSum = 0;
                if(velocityNeeded < MaxSpeed)
                {
                    velocityNeeded += SpeedInterval;
                }
                else
                {
                    SceneManager.LoadScene(nextScene);
                }
            }

            if (GetComponent<Rigidbody2D>().velocity.magnitude < velocityNeeded)
            {
                timeUnderReq += Time.deltaTime;
            }

            if (timeUnderReq >= timeAllowedUnderReq)
            {
                SceneManager.LoadScene("Fail");
            }



        }
	}
}
