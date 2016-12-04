using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// short class to keep focus on a textbox; used on victory scene.
/// </summary>
public class FocusHolder : MonoBehaviour {

    public GameObject TextBox;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(TextBox);
        }
    }
}
