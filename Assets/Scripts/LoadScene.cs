using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string scene;

    public void Start()
    {
        Button b = GetComponent<Button>();

        b.onClick.AddListener(delegate () { Load(scene); });
    }
    public void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
