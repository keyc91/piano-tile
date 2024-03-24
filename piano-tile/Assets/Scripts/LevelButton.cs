using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class LevelButton : MonoBehaviour
{
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
        // jm�no levelu z rodi�e, pop��pad� z game objectu samotn�ho
        string scene = gameObject.transform.parent.name;
        if (scene == "Container")
        {
            scene = gameObject.name;
        }

        // ulo�en� jm�na vybran�ho levelu
        PlayerPrefs.SetString("CurrentLevel", scene);
        
        // spu�ten� animace
        LevelLoader.Instance.animator.SetTrigger("Scene");

        // spu�t�n� levelu po uplynut� jedn� vte�iny 
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(2);
    }
}
