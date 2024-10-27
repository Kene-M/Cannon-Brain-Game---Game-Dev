using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// For transitioning between scenes/screens
public class ButtonTest : MonoBehaviour
{
    public void ChangeScenes(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
