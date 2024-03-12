using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  


public class LevelsLevelButton : MonoBehaviour
{
    public static LevelsLevelButton Instance;

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
        PlayerPrefs.SetInt("CurrentLevel", int.Parse(transform.parent.gameObject.name));
        SceneManager.LoadScene(transform.parent.gameObject.name);
    }
}
