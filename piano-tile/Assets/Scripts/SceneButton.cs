using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    public static SceneButton Instance;
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
        LevelLoader.Instance.animator.SetTrigger("Scene");
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(scene);
    }
}