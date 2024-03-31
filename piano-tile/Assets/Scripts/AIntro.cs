using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AIntro : MonoBehaviour
{
    public float frameRate;
    public bool looping;

    private Image image;
    private Sprite[] sprites;
    private float timePerFrame;
    private int currentFrame;
    private float lastCallTime;


    void Start()
    {
        // inicializace indexu aktuálního snímku
        currentFrame = 0;

        // získání reference na Image komponent objektu
        image = GetComponent<Image>();

        // naètení spriteù animace
        LoadSpriteSheet();
    }

    void Update()
    {
        // aktualizace snímku pouze pokud uplynul dostateèný èas
        if (Time.timeSinceLevelLoad - (timePerFrame) >= lastCallTime)
        {
            // aktualizace èasu posledního vykreslení
            lastCallTime = Time.timeSinceLevelLoad;

            // pøechod na další snímek
            currentFrame++;

            // pokud jsme prošli všechny snímky
            if (currentFrame >= sprites.Length)
            {
                // pokud je zapnutý looping a zobrazuje se poslední snímek, naèteme nové snímky
                if (looping && image.sprite.name == "intro_19")
                {
                    sprites = new Sprite[0];
                    sprites = Resources.LoadAll<Sprite>("Sprites/IntroLoop");
                    currentFrame = 0;
                }
                // jinak se vrátíme na první snímek
                else
                {
                    image.sprite = sprites[0];
                    currentFrame = 0;
                }
            }

            // nastavení aktuálního snímku
            SetSprite();
        }
    }

    // naètení spritù animace
    private void LoadSpriteSheet()
    {
        // naètení spriteù z adresáøe "Sprites/Intro"
        sprites = Resources.LoadAll<Sprite>("Sprites/Intro");

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
            // nastavení snímku pro zobrazení
            image.sprite = sprites[currentFrame];
        }
    }
}

