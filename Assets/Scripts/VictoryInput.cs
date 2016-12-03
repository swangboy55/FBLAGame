using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class VictoryInput : MonoBehaviour
{
    public GameObject NameField;
    public Text ScoreField;
    private int finalScore;
    private float scoreAnimStart;
    private bool firstUpdate = false;
    private bool animDone = false;

    // Use this for initialization
    void Start ()
    {
        float finalScoref = 0;
        foreach (float score in ScorePersistence.LevelScores)
        {
            finalScoref += score;
        }
        ScorePersistence.LevelScores.Clear();
        finalScore = (int)Mathf.Round(finalScoref);
        finalScore = 105351;
    }

    //number of entries in the scoreboard. I'd rather keep it at 20. 
    public int numScores = 20;

    /// <summary>
    /// stores a score permenently in memory, located in game folder as txt
    /// INTENDED FUNCTIONALITY: Player CAN store multiple scores in the leaderboard of their own.
    /// # of scores is fixed
    /// </summary>
    /// <param name="name">name of the player</param>
    /// <param name="score">score that the player got</param>
    void Store(string name, int score)
    {
        string entry = name + " : " + score;
        string[] lines = new string[20];

        //makes a new scoreboard and fills it with the entry if a scoreboard file does not exist.
        if (!System.IO.File.Exists("scoreboard.txt"))
        {
            string[] Filllines = new string[20];
            for (int i = 0; i < Filllines.Length; i++)
            {
                Filllines[i] = " : 0";
            }
            Filllines[0] = entry;
            System.IO.File.WriteAllLines("scoreboard.txt", Filllines);
        }
        //scoreboard does exist
        else
        {
            lines = System.IO.File.ReadAllLines("scoreboard.txt");
            int i;
            for (i = 0; i < lines.Length; i++)
            {
                if (lines[i] == " : ")
                {
                    break;
                }
                if (score > int.Parse((lines[i].Substring(lines[i].IndexOf(':') + 2))))
                {
                    break;
                }
            }

            for (int a = 19; a > i; a--)
            {
                lines[a] = lines[a - 1];
            }
            if (i < 20)
            {
                lines[i] = entry;
            }

            System.IO.File.WriteAllLines("scoreboard.txt", lines);


        }


    }

    // ANIMatION loL
    void Update () {

        if(Input.GetKey(KeyCode.Return))
        {
            Store(NameField.GetComponent<BetterInputField>().text, finalScore);
            UnityEngine.SceneManagement.SceneManager.LoadScene("leaderboard");
        }

        if(animDone)
        {
            return;
        }

        if (firstUpdate)
        {
            firstUpdate = false;
            scoreAnimStart = Time.time;
        }
        //mask real numbers over constantly changing rand numbers left->right
        StringBuilder rngBuilder = new StringBuilder();

        for(int a=0;a<finalScore.ToString().Length;a++)
        {
            rngBuilder.Append(Mathf.Round(Random.Range(0, 9)));
        }

        string realNumbers = finalScore.ToString();

        int digitsPassed = (int)Mathf.Min(Mathf.Max(((Time.time - scoreAnimStart - 0.5f) / 0.25f), 0), realNumbers.Length);

        StringBuilder finalString = new StringBuilder();

        for(int a=0;a<digitsPassed;a++)
        {
            finalString.Append(realNumbers[a]);
        }
        for(int a=digitsPassed;a<rngBuilder.Length;a++)
        {
            finalString.Append(rngBuilder[a]);
        }
        ScoreField.text = finalString.ToString();

        if (digitsPassed == realNumbers.Length)
        {
            animDone = true;
            NameField.GetComponent<InputField>().Select();
            NameField.GetComponent<InputField>().ActivateInputField();
        }

    }
}
