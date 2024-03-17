using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class LevelButton : MonoBehaviour
{
    public string scene;

    void Start()
    {
        // nacteni buttonu
        Button yourButton = GetComponent<Button>();
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        // ulozeni jmena vybraneho levelu
        PlayerPrefs.SetString("CurrentLevel", scene);
        
        // trigger animace
        LevelLoader.Instance.animator.SetTrigger("Scene");

        // spusteni levelu po uplynuti jedne vteriny 
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(2);
    }
}
