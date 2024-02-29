using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        // SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
        SceneManager.LoadScene(0);
    }

    /*public void Hit()
    {
        // SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
        SceneManager.LoadScene(0);

    }*/
}