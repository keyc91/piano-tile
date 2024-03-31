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
        // nastav�me po��te�n� sn�mek na 0
        currentFrame = 0;

        // z�sk�me odkaz na komponent Image
        image = GetComponent<Image>();

        // na�teme sprite sheet
        LoadSpriteSheet();
    }

    void Update()
    {
        // aktualizujeme sn�mek pokud uplynul ur�it� �as
        if (Time.timeSinceLevelLoad - timePerFrame >= lastCallTime)
        {
            // ulo��me �as posledn� aktualizace
            lastCallTime = Time.timeSinceLevelLoad;

            // p�echod na dal�� sn�mek
            currentFrame++;

            // pokud jsme pro�li v�echny sn�mky
            if (currentFrame >= sprites.Length)
            {
                // pokud je povoleno opakov�n�
                if (looping)
                {
                    // nastav�me prvn� sn�mek a resetujeme po��tadlo
                    image.sprite = sprites[0];
                    currentFrame = 0;
                }
                else
                {
                    // jinak zastav�me tohoto scriptu
                    enabled = false;
                }
            }

            // nastav�me aktu�ln� sn�mek
            SetSprite();
        }
    }

    private void LoadSpriteSheet()
    {
        // na�teme v�echny sprity ze sprite sheetu
        sprites = Resources.LoadAll<Sprite>("Sprites/" + gameObject.name);

        // vypo��t�me �as na jeden sn�mek
        timePerFrame = 1f / frameRate;

        // nastav�me po��te�n� sn�mek
        SetSprite();
    }

    private void SetSprite()
    {
        // kontrola platn�ho indexu sn�mku
        if (currentFrame >= 0 && currentFrame < sprites.Length)
        {
            // nastav�me aktu�ln� sn�mek
            image.sprite = sprites[currentFrame];
        }
    }
}
