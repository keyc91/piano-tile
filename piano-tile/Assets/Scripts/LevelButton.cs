using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class LevelButton : MonoBehaviour
{
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
        // jméno levelu z rodièe, popøípadì z game objectu samotného
        string scene = gameObject.transform.parent.name;
        if (scene == "Container")
        {
            scene = gameObject.name;
        }

        // uložení jména vybraného levelu
        PlayerPrefs.SetString("CurrentLevel", scene);
        
        // spuštení animace
        LevelLoader.Instance.animator.SetTrigger("Scene");

        // spuštìní levelu po uplynutí jedné vteøiny 
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(2);
    }
}
