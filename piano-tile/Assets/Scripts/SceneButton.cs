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
        // naètení tlaèítka
        Button yourButton = GetComponent<Button>(); 
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        // spuštìní animace
        LevelLoader.Instance.animator.SetTrigger("Scene");

        // spuštení scény po uplynutí jedné vteøiny
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(scene);
    }
}