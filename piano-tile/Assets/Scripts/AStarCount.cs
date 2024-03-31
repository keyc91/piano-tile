using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AStarCount : MonoBehaviour
{
    public float frameRate;
    private Image image;

    private Sprite[] sprites;
    private float timePerFrame;
    private int currentFrame;
    private float lastCallTime;

    void Start()
    {
        // index aktuálního snímku
        currentFrame = 0;

        // získání reference na Image komponent objektu
        image = GetComponent<Image>();

        // naètení spritù animace
        LoadSpriteSheet();
    }

    void Update()
    {
        // aktualizace snímku - pokud uplynul dostateèný èas
        if (Time.timeSinceLevelLoad - (timePerFrame) >= lastCallTime)
        {
            // aktualizace èasu posledního vykreslení
            lastCallTime = Time.timeSinceLevelLoad;

            // pøechod na další snímek
            currentFrame++;

            // pokud jsme prošli všechny snímky
            if (currentFrame >= sprites.Length)
            {
                // zablokování komponenty AStarCount
                enabled = false;
            }

            // nastavení aktuálního snímku
            SetSprite();
        }
    }

    // aaètení spritù animace
    private void LoadSpriteSheet()
    {
        string path;

        // urèení cesty podle aktuální scény
        if (SceneManager.GetActiveScene().name == "levels menu")
        {
            // název rodièe
            string parentsName = transform.parent.gameObject.name;

            // naètení poètu hvìzd pro daný level
            int stars = PlayerPrefs.GetInt("Level" + parentsName + "Stars");

            // vytvoøení cesty k spritùm menu
            path = stars + "starsmenu";
        }
        else
        {
            // naètení poètu aktuálních hvìzd
            path = PlayerPrefs.GetInt("CurrentStars") + "gameover";
        }

        // naètení spritù podle cesty
        sprites = Resources.LoadAll<Sprite>("Sprites/" + path);

        // výpoèet èasu na jeden snímek animace
        timePerFrame = 1f / frameRate;

        // nastavení poèáteèního snímku
        SetSprite();
    }

    // nastavení aktuálního snímku
    private void SetSprite()
    {
        // kontrola platného rozsahu indexu
        if (currentFrame >= 0 && currentFrame < sprites.Length)
        {
            // astavení snímku pro zobrazení
            image.sprite = sprites[currentFrame];
        }
    }
}
