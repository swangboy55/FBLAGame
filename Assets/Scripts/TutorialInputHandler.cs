using UnityEngine;
using System.Collections;
using UnityEngine.UI;


// Script to handle user input on the controls/tutorial screen
public class TutorialInputHandler : MonoBehaviour
{


    private bool keyState = false;

    void Start()
    {

    }

    /// <summary>
    /// on pressing enter: if player is on controls1, move to controls2. Otherwise, go to menu
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
