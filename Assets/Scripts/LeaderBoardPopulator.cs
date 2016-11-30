using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Script to populate leaderboard with saved values and display them on the scene
public class LeaderBoardPopulator : MonoBehaviour
{

    public Text Entry1;
    public Text Entry2;
    public Text Entry3;
    public Text Entry4;
    public Text Entry5;
    public Text Entry6;
    public Text Entry7;
    public Text Entry8;
    public Text Entry9;
    public Text Entry10;
    public Text Entry11;
    public Text Entry12;
    public Text Entry13;
    public Text Entry14;
    public Text Entry15;
    public Text Entry16;
    public Text Entry17;
    public Text Entry18;
    public Text Entry19;
    public Text Entry20;


    /// <summary>
    /// Make an array of all entries, get the lines from the textfile, and transfer each line in textfile to the entry. 
    /// The leaderbaord will be populated with blank entries consisting of a space for a name and 0 as the score if the scoreboard does not exist.(assumed to be the case on first run)
    /// 
    /// </summary>
    void Start()
    {
        string[] writeLines = new string[20];
        string[] scoreBoardLines = new string[20];
        if (!System.IO.File.Exists("scoreboard.txt"))
        {
            for (int i = 0; i <= 19; i++)
            {
                writeLines[i] = (i + 1) + " -   : 0";
            }
        }
        else
        {
            scoreBoardLines = System.IO.File.ReadAllLines("scoreboard.txt");
            for (int i = 0; i <= 19; i++)
            {
                writeLines[i] = (i + 1) + " - " + scoreBoardLines[i];
            }

        }

        Text[] leaderBoardEntries = {Entry1, Entry2, Entry3, Entry4, Entry5, Entry6, Entry7, Entry8, Entry9, Entry10,
        Entry11, Entry12, Entry13, Entry14, Entry15, Entry16, Entry17, Entry18, Entry19, Entry20};

        for (int i = 0; i <= 19; i++)
        {
            leaderBoardEntries[i].GetComponent<UnityEngine.UI.Text>().text = writeLines[i];
        }

    }



    // Update is called once per frame
    void Update()
    {
        //unused
    }


}