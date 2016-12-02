using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameHandler : MonoBehaviour
{

    public float MinSpeed;
    public float MaxSpeed;
    public float PreparationSeconds = 10.0f;
    public float timeAllowedUnderReq = 5f;
    public GameObject speedReq;
    public GameObject playerVel;
    public GameObject secondsTillDead;
    public GameObject failPrompt;
    public string nextScene;
    
    public float SpeedInterval;
    public float SpeedIntervalRate = 1.0f;

    private string currentScene;
    private float velocityUpperBound;
    private float score;
    private float timeStart;
    private float velocityNeeded;
    private float timeUnderReq = 0;
    private float deltaTimeSum = 0;
    private int combo;

    private static int lives = 3; 
    private float velCurHue;
    private float reqCurHue;

    public float GetScore()
    {
        return score;
    }

    // Use this for initialization
    void Start()
    {
        timeStart = Time.time;
        velocityNeeded = MinSpeed;
        velocityUpperBound = GetComponent<BallColorChanger>().VelocityUpperBound;
        velCurHue = 0.8f;
        reqCurHue = 0.8f;
        currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude + " , " + velocityNeeded + " , " + timeUnderReq);
        if (Time.time < timeStart + PreparationSeconds)
        {
            if(Time.time < timeStart + PreparationSeconds / 3)
            {
                failPrompt.GetComponent<UnityEngine.UI.Text>().enabled = true;
            }
            else
            {
                failPrompt.GetComponent<UnityEngine.UI.Text>().enabled = false;
            }
        }
        else
        {
            deltaTimeSum += Time.deltaTime;
            if (deltaTimeSum >= SpeedIntervalRate)
            {
                deltaTimeSum = 0;
                if (velocityNeeded < MaxSpeed)
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
                if(lives > 0)
                {
                    SceneManager.LoadScene(currentScene);
                    lives--;
                }
                else
                {
                    SceneManager.LoadScene("fail");
                    lives = 3;
                }
                
            }

        }
        velCurHue = (GetComponent<Rigidbody2D>().velocity.magnitude / velocityUpperBound);
        if (velCurHue > 1)
        {
            velCurHue = 1;
        }
        velCurHue *= 0.8f;
        velCurHue = 0.8f - velCurHue;
        Color playerVelColor = Color.HSVToRGB(velCurHue, 1, 1);
        playerVelColor.a = 88.0f / 255.0f;
        playerVel.GetComponent<UnityEngine.UI.Image>().color = playerVelColor;
        reqCurHue = (velocityNeeded / MaxSpeed);
        if (reqCurHue > 1)
        {
            reqCurHue = 1;
        }
        reqCurHue *= 0.8f;
        reqCurHue = 0.8f - reqCurHue;
        Color velNeededColor = Color.HSVToRGB(reqCurHue, 1, 1);
        velNeededColor.a = 88.0f / 255.0f;
        speedReq.GetComponent<UnityEngine.UI.Image>().color = velNeededColor;
        {
            Vector3 localScale = playerVel.GetComponent<UnityEngine.UI.Image>().transform.localScale;
            localScale.Set(localScale.x, ((0.8f - velCurHue) * 1.25f * 6f) + 1f, localScale.z);
            playerVel.GetComponent<UnityEngine.UI.Image>().transform.localScale = localScale;
        }
        {
            Vector3 localScale = speedReq.GetComponent<UnityEngine.UI.Image>().transform.localScale;
            localScale.Set(localScale.x, (((velocityNeeded - MinSpeed) / (MaxSpeed - MinSpeed)) * 6f) + 1f, localScale.z);
            speedReq.GetComponent<UnityEngine.UI.Image>().transform.localScale = localScale;
        }

        secondsTillDead.GetComponent<UnityEngine.UI.Text>().text = ((float)((int)(timeUnderReq * 100.0f)) / 100.0f).ToString() + " / " + timeAllowedUnderReq;

        failPrompt.GetComponent<UnityEngine.UI.Text>().text = "Lives : " + lives;






    }

    void updateCombo(bool reset)
    {
        if (!reset)
        {
            combo++;
        }
        else
        {
            combo = 0;
        }


    }


    void updateScore(float velocityOver)
    {
        //score += som e s h it *combo;
    }
}
