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
        // inicializace indexu aktu�ln�ho sn�mku
        currentFrame = 0;

        // z�sk�n� reference na Image komponent objektu
        image = GetComponent<Image>();

        // na�ten� sprite� animace
        LoadSpriteSheet();
    }

    void Update()
    {
        // aktualizace sn�mku pouze pokud uplynul dostate�n� �as
        if (Time.timeSinceLevelLoad - (timePerFrame) >= lastCallTime)
        {
            // aktualizace �asu posledn�ho vykreslen�
            lastCallTime = Time.timeSinceLevelLoad;

            // p�echod na dal�� sn�mek
            currentFrame++;

            // pokud jsme pro�li v�echny sn�mky
            if (currentFrame >= sprites.Length)
            {
                // pokud je zapnut� looping a zobrazuje se posledn� sn�mek, na�teme nov� sn�mky
                if (looping && image.sprite.name == "intro_19")
                {
                    sprites = new Sprite[0];
                    sprites = Resources.LoadAll<Sprite>("Sprites/IntroLoop");
                    currentFrame = 0;
                }
                // jinak se vr�t�me na prvn� sn�mek
                else
                {
                    image.sprite = sprites[0];
                    currentFrame = 0;
                }
            }

            // nastaven� aktu�ln�ho sn�mku
            SetSprite();
        }
    }

    // na�ten� sprit� animace
    private void LoadSpriteSheet()
    {
        // na�ten� sprite� z adres��e "Sprites/Intro"
        sprites = Resources.LoadAll<Sprite>("Sprites/Intro");

        // v�po�et �asu na jeden sn�mek animace
        timePerFrame = 1f / frameRate;

        // nastaven� po��te�n�ho sn�mku
        SetSprite();
    }

    // nastaven� aktu�ln�ho sn�mku
    private void SetSprite()
    {
        // kontrola platn�ho rozsahu indexu
        if (currentFrame >= 0 && currentFrame < sprites.Length)
        {
            // nastaven� sn�mku pro zobrazen�
            image.sprite = sprites[currentFrame];
        }
    }
}

