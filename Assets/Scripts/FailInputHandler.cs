using UnityEngine;
using System.Collections;
using UnityEngine.UI;


// Script to handle user input on the failure screen. Very similar to the MenuInputHandler, but has slight
// differences in terms of how it works and how many buttons it supports.
public class FailInputHandler : MonoBehaviour
{
    public Button ButtonOne, ButtonTwo;
    public Canvas canvas;
    public GameObject selectorPrefab;
    private GameObject selector;
    private Button[] buttons;
    private int buttonIndex = 0;
    private bool keyState = false;
    /// <summary>
    /// Make an array with all menu buttons passed in, and then move the selector to the first button
    /// </summary>
    void Start()
    {
        ButtonOne.enabled = false;
        ButtonTwo.enabled = false;        
        buttons = new Button[] { ButtonOne, ButtonTwo };
        selector = Instantiate(selectorPrefab) as GameObject;
        selector.transform.SetParent(canvas.transform);
        selector.transform.localScale = new Vector3(1, 1, 1);
        selector.transform.localPosition = new Vector3(ButtonOne.transform.localPosition.x - 400, ButtonOne.transform.localPosition.y,
            0);
    }

    /// <summary>
    /// Move the selector if user input is given to the different buttons, and invoke an onClick event if enter is pressed
    /// on a given button.
    /// </summary>
    void OnGUI()
    {
        if(ButtonOne == null || ButtonTwo == null)
        {
            return;
        }

        if (Input.GetKey("down"))
        {
            if (!keyState)
            {
                buttonIndex++;
                if (buttonIndex >= 2)
                {
                    buttonIndex = 0;
                }
                selector.transform.localPosition = new Vector3(buttons[buttonIndex].transform.localPosition.x - 400, buttons[buttonIndex].transform.localPosition.y,
                    0);
            }
            keyState = true;
        }
        else if (Input.GetKey("up"))
        {
            if (!keyState)
            {
                buttonIndex--;
                if (buttonIndex < 0)
                {
                    buttonIndex = 1;
                }
                selector.transform.localPosition = new Vector3(buttons[buttonIndex].transform.localPosition.x - 400, buttons[buttonIndex].transform.localPosition.y,
                    0);
            }
            keyState = true;
        }
        else if(Input.GetKey(KeyCode.Return))
            {
                if (!keyState)
                {
                    buttons[buttonIndex].onClick.Invoke();
                }
            keyState = true;
        }
        else
        {
            keyState = false;
        }
    }
}
