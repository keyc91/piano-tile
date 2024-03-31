using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AGameOver : MonoBehaviour
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
        // nastavíme poèáteèní snímek na 0
        currentFrame = 0;

        // získáme odkaz na komponent Image
        image = GetComponent<Image>();

        // naèteme sprite sheet
        LoadSpriteSheet();
    }

    void Update()
    {
        // aktualizujeme snímek pokud uplynul urèitý èas
        if (Time.timeSinceLevelLoad - timePerFrame >= lastCallTime)
        {
            // uložíme èas poslední aktualizace
            lastCallTime = Time.timeSinceLevelLoad;

            // pøechod na další snímek
            currentFrame++;

            // pokud jsme prošli všechny snímky
            if (currentFrame >= sprites.Length)
            {
                // pokud je povoleno opakování
                if (looping)
                {
                    // nastavíme první snímek a resetujeme poèítadlo
                    image.sprite = sprites[0];
                    currentFrame = 0;
                }
                else
                {
                    // jinak zastavíme tohoto scriptu
                    enabled = false;
                }
            }

            // nastavíme aktuální snímek
            SetSprite();
        }
    }

    private void LoadSpriteSheet()
    {
        // naèteme všechny sprity ze sprite sheetu
        sprites = Resources.LoadAll<Sprite>("Sprites/" + gameObject.name);

        // vypoèítáme èas na jeden snímek
        timePerFrame = 1f / frameRate;

        // nastavíme poèáteèní snímek
        SetSprite();
    }

    private void SetSprite()
    {
        // kontrola platného indexu snímku
        if (currentFrame >= 0 && currentFrame < sprites.Length)
        {
            // nastavíme aktuální snímek
            image.sprite = sprites[currentFrame];
        }
    }
}
