﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
