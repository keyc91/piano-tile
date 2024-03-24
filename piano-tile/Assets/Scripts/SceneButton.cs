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
        // na�ten� tla��tka
        Button yourButton = GetComponent<Button>(); 
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        // spu�t�n� animace
        LevelLoader.Instance.animator.SetTrigger("Scene");

        // spu�ten� sc�ny po uplynut� jedn� vte�iny
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(scene);
    }
}