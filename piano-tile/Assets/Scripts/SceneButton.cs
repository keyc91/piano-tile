using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
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
        // trigger animace
        LevelLoader.Instance.animator.SetTrigger("Scene");

        // spusteni sceny po uplynuti jedne vteriny
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(scene);
    }
}