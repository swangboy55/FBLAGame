using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuInputHandler : MonoBehaviour
{
    public Button ButtonOne, ButtonTwo, ButtonThree;
    public Canvas canvas;
    public GameObject selectorPrefab;
    private GameObject selector;
    private Button[] buttons;
    private int buttonIndex = 0;
    private bool keyState = false;

    void Start()
    {
        ButtonOne.enabled = false;
        ButtonTwo.enabled = false;
        ButtonThree.enabled = false;
        buttons = new Button[] { ButtonOne, ButtonTwo, ButtonThree };
        selector = Instantiate(selectorPrefab) as GameObject;
        selector.transform.SetParent(canvas.transform);
        selector.transform.localScale = new Vector3(1, 1, 1);
        selector.transform.localPosition = new Vector3(ButtonOne.transform.localPosition.x - 400, ButtonOne.transform.localPosition.y,
            0);
    }

    void OnGUI()
    {
        if(ButtonOne == null || ButtonTwo == null || ButtonThree == null)
        {
            return;
        }

        if (Input.GetKey("down"))
        {
            if (!keyState)
            {
                buttonIndex++;
                if (buttonIndex >= 3)
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
                    buttonIndex = 2;
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
