using UnityEngine;
using System.Collections;

//Script to permenently store player scores for the leaderboard.
public class StoreScore : MonoBehaviour
{

    //number of entries in the scoreboard. I'd rather keep it at 20. 
    public int numScores = 20;
    // Use this for initialization    
    void Start()
    {
        //unused
    }

    // Update is called once per frame
    void Update()
    {
        //unused
    }

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
                Filllines[i] = " : ";
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
}


