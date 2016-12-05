using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


/// <summary>
/// Class/Script to handle basically everything that actually happens during gameplay in context of scoring, winning and losing, updating combo and score, etc.
/// </summary>
public class GameHandler : MonoBehaviour
{
    //long list of neccecary variables to keep the game running. Lives is static because it must persist.
    public float MinSpeed;
    public float MaxSpeed;
    public float PreparationSeconds = 10.0f;
    public float timeAllowedUnderReq = 5f;
    public GameObject Music;
    public GameObject speedReq;
    public GameObject playerVel;
    public GameObject secondsTillDead;
    public GameObject failPrompt;
    public GameObject comboObject;
    public GameObject scoreObject;
    public string nextScene;

    public static int lives = 3;

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

   
    private string level;
    private float velCurHue;
    private float reqCurHue;

    //accessor method to score
    public float GetScore()
    {
        return score;
    }

    // Use this for initialization
    //Set up all starting conditions for a level.
    void Start()
    {
        timeStart = Time.time;
        velocityNeeded = MinSpeed;
        velocityUpperBound = GetComponent<BallColorChanger>().VelocityUpperBound;
        velCurHue = 0.8f;
        reqCurHue = 0.8f;
        currentScene = SceneManager.GetActiveScene().name;
        level = currentScene.Substring(5);

        
    }

    // Update is called once per frame
    /// <summary>
    /// in update, pretty much every variable is checked and updated; and HUD elements are updated with current info.
    /// </summary>
    void Update()
    {

        //give the player time to prepare, tick down the timer for preparation, and in the first third of preparation time, show the lives and level.
        if (Time.time < timeStart + PreparationSeconds)
        {

            secondsTillDead.GetComponent<UnityEngine.UI.Text>().color = Color.HSVToRGB(118.0f / 255.0f, 1, 1);
            secondsTillDead.GetComponent<UnityEngine.UI.Text>().text = "Prepare in:" + "\n" + ((float)((int)((PreparationSeconds + timeStart - Time.time) * 100.0f)) / 100.0f).ToString();

            //3 is just an arbitrary value for what percentage of time the dialouge should show
            if (Time.time < timeStart + PreparationSeconds / 3)
            {
                failPrompt.GetComponent<UnityEngine.UI.Text>().enabled = true;
            }
            else
            {
                failPrompt.GetComponent<UnityEngine.UI.Text>().enabled = false;
            }
        }
        //the game has now officially started, and the velocity needed will start to increase.
        else
        {
            //play music when the game begins
            if (!Music.GetComponent<AudioSource>().isPlaying)
            {
                Music.GetComponent<AudioSource>().Play();
            }
            
            //make the seconds under req element red and start adding to deltatimesum. Deltatime sum serves as a conversion from updates->seconds. VelocityNeeded is updated every 1 * speedInterval seconds
            //if velocityNeeded is greater than maxspeed, the player has won, so the next Scene is loaded.
            secondsTillDead.GetComponent<UnityEngine.UI.Text>().color = Color.HSVToRGB(0, 1, 1);
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
                    if(nextScene.Equals("Victory"))
                    {
                        lives = 3;
                    }
                }
            }

            //Add to time under req if the player is going under required velocity
            if (GetComponent<Rigidbody2D>().velocity.magnitude < velocityNeeded)
            {
                timeUnderReq += Time.deltaTime;
            }

            //If the player exceeds the time allowed under requirement, either take a life from them and restart the level, or end the game if they are out of lives.
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

        //Calculate the colors of the speed indicators on the left and right of the screen, as well as their sizes.
        //Numbers found mathematically based on what expected conditions for these were to be.
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

        //Update Time under req HUD element, assuming it needs to be updated. Also rounds the float value to the nearest hundredth
        if (Time.time >= timeStart + PreparationSeconds)
        {

            secondsTillDead.GetComponent<UnityEngine.UI.Text>().text = "Time under:" + "\n" + ((float)((int)(timeUnderReq * 100.0f)) / 100.0f).ToString() + " / " + timeAllowedUnderReq;
        }

        //Update level/life counter on hud/ combo meter, and score.
        failPrompt.GetComponent<UnityEngine.UI.Text>().text = "Level: " + level + "\n" + "Lives : " + lives;
        comboObject.GetComponent<UnityEngine.UI.Text>().text = combo + "x";
        scoreObject.GetComponent<UnityEngine.UI.Text>().text = "Score:" + "\n" +((long)score).ToString();
    }

    //scale of the combo meter(it gets bigger over time)
    private float baseScale = 1;
    //Update combo
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
    
    //Update timer(assuming changes need to be made to it: timer platform)
    public void UpdateTimer(float bonusTime)
    {
        timeUnderReq -= bonusTime;
        if(timeUnderReq < 0)
        {
            timeUnderReq = 0;
        }
    }
    
    //Neat animation whenever combo updates. Completley aesthetic
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

    //Update the score; based on velocity over requirement times combo whenever combo is updated. Called in PlayerController.
    public void UpdateScore(float velocity)
    {
        //combo is 0 based (0 combo should add 1x the score)
        score += (velocity - velocityNeeded) * (combo + 1);
        score = Mathf.Round(score);
    }

    //amt range : (1, 2] Multiply score by some amount when needed: score platform.
    public void MulScore(float amt)
    {
        if(amt <= 1 || amt > 2)
        {
            return;
        }
        score *= amt;
    }
}