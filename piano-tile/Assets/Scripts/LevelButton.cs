using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  


public class LevelButton : MonoBehaviour
{
    public static LevelButton Instance;
    public string scene;

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
        PlayerPrefs.SetString("CurrentLevel", scene);
        LevelLoader.Instance.animator.SetTrigger("Scene");
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(2);
    }
}
