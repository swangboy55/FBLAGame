using UnityEngine;
using System.Collections;
using UnityEngine.UI;


// Script to handle user input on the failure screen. Very similar to the MenuInputHandler, but has slight
// differences in terms of how it works and how many buttons it supports.
public class TutorialInputHandler : MonoBehaviour
{


    private bool keyState = false;

    void Start()
    {

    }

    /// <summary>
    /// Load menu scene when player hits enter
    /// </summary>
    void OnGUI()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            if (!keyState)
            {
                if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "controls1" )
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("controls2");
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
                }
                
            }
            keyState = true;
        }
        else
        {
            keyState = false;
        }
    }
}
