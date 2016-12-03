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
    public GameObject comboObject;
    public GameObject scoreObject;
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
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude + " , " + velocityNeeded + " , " + timeUnderReq);
        if (Time.time < timeStart + PreparationSeconds)
        {
            if (Time.time < timeStart + PreparationSeconds / 3)
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
                    ScorePersistence.LevelScores.Add(score);
                    SceneManager.LoadScene(nextScene);
                }
            }

            if (GetComponent<Rigidbody2D>().velocity.magnitude < velocityNeeded)
            {
                timeUnderReq += Time.deltaTime;
            }

            if (timeUnderReq >= timeAllowedUnderReq)
            {
                lives--;
                if (lives > 0)
                {
                    SceneManager.LoadScene(currentScene);
                }
                else
                {
                    lives = 3;
                    SceneManager.LoadScene("fail");
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
        comboObject.GetComponent<UnityEngine.UI.Text>().text = combo + "x";
    }


    private float baseScale = 1;

    public void UpdateCombo(bool reset)
    {
        if (!reset)
        {
            combo++;
        }
        else
        {
            combo = 0;
        }
        baseScale = Mathf.Min(combo / 100.0f, 1) + 1;
    }

    public void UpdateTimer(float bonusTime)
    {
        timeUnderReq -= bonusTime;
        if(timeUnderReq < 0)
        {
            timeUnderReq = 0;
        }
    }

    public void UpdateComboPretty(float TWI, float IT)
    {
        IT /= 8;
        if(IT - TWI <= 0)
        {
            comboObject.transform.localScale = new Vector3(baseScale, baseScale, 1);
            comboObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            float scaleUp = baseScale + ((IT - TWI) / (IT * 2));
            comboObject.transform.localScale = new Vector3(scaleUp, scaleUp, 1);
            comboObject.transform.rotation = Quaternion.Euler(0, 0, (scaleUp - baseScale) * 45);
        }
    }

    public void UpdateScore(float velocity)
    {
        //combo is 0 based (0 combo should add 1x the score)
        score += (velocity - velocityNeeded) * (combo + 1);
    }

    //amt range : (1, 2]
    public void MulScore(float amt)
    {
        if(amt <= 1 || amt > 2)
        {
            return;
        }
        score *= amt;
    }
}