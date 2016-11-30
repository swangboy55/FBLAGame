using UnityEngine;
using System.Collections;
using UnityEngine.UI;


// Script to handle user input on the failure screen. Very similar to the MenuInputHandler, but has slight
// differences in terms of how it works and how many buttons it supports.
public class LeaderBoardInputHandler : MonoBehaviour
{
    public Button ButtonOne;
    public Canvas canvas;
    public GameObject selectorPrefab;
    private GameObject selector;
    private bool keyState = false;
    /// <summary>
    /// Make an array with all menu buttons passed in, and then move the selector to the first button
    /// </summary>
    void Start()
    {
        ButtonOne.enabled = false;       
        selector = Instantiate(selectorPrefab) as GameObject;
        selector.transform.SetParent(canvas.transform);
        selector.transform.localScale = new Vector3(1, 1, 1);
        selector.transform.localPosition = new Vector3(ButtonOne.transform.localPosition.x - 110, ButtonOne.transform.localPosition.y,
            0);
    }

    /// <summary>
    /// If Player hits enter, invoke buttonOnes onClick event
    /// </summary>
    void OnGUI()
    {
        if(ButtonOne == null)
        {
            return;
        }
        if(Input.GetKey(KeyCode.Return))
        {
            if (!keyState)
            {
                ButtonOne.onClick.Invoke();
            }
            keyState = true;
        }
        else
        {
            keyState = false;
        }
    }
}
