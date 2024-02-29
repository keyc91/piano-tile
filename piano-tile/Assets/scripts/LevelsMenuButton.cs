using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenuButton : MonoBehaviour
{
    public static LevelButton Instance;

    void Start()
    {
        Button yourButton = GetComponent<Button>();
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        SceneManager.LoadScene(2);
    }
}