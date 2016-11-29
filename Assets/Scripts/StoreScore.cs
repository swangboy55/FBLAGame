using UnityEngine;
using System.Collections;

public class StoreScore : MonoBehaviour {

    //number of entries in the scoreboard. I'd rather keep it at 20. 
    public int numScores = 20;
    // Use this for initialization
    
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void Store(string name, int score)
    {
        string[] lines = new string[20];
        if (!System.IO.File.Exists("scoreboard.txt"))
        {
            string[] Filllines = new string[20];
            for (int i = 0; i < Filllines.Length; i++)
            {
                lines[i] = " : ";
            }
            System.IO.File.WriteAllLines("scoreboard.txt", Filllines);
        }
        else
        {
            lines = System.IO.File.ReadAllLines("scoreboard.txt");
            string entry = name + " : " + score;
            int i;
            for (i =0; i < lines.Length; i++)
            {
                if(lines[i] == " : " )
                {
                    break;
                }
                if(score > int.Parse((lines[i].Substring(lines[i].IndexOf(':') + 2))))
                {
                    break;
                }
            }

            for (int a=i; a<19; a++)
            {
                lines[a + 1] = lines[a];                
            }
            if(i < 20)
            {
                lines[i] = entry;
            }

            System.IO.File.WriteAllLines("scoreboard.txt", lines);


        }

       
    }

}
