using UnityEngine;
using System.Collections;
using UnityEngine.UI;


// Script to handle input on leaderboard.
public class LeaderBoardInputHandler : MonoBehaviour
{
    public Button ButtonOne;
    public Canvas canvas;
    public GameObject selectorPrefab;
    private GameObject selector;
    private bool keyState = false;
    /// <summary>
    /// Move selector to the button passed in
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
