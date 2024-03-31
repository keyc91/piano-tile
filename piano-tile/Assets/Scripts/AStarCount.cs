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
        // index aktu�ln�ho sn�mku
        currentFrame = 0;

        // z�sk�n� reference na Image komponent objektu
        image = GetComponent<Image>();

        // na�ten� sprit� animace
        LoadSpriteSheet();
    }

    void Update()
    {
        // aktualizace sn�mku - pokud uplynul dostate�n� �as
        if (Time.timeSinceLevelLoad - (timePerFrame) >= lastCallTime)
        {
            // aktualizace �asu posledn�ho vykreslen�
            lastCallTime = Time.timeSinceLevelLoad;

            // p�echod na dal�� sn�mek
            currentFrame++;

            // pokud jsme pro�li v�echny sn�mky
            if (currentFrame >= sprites.Length)
            {
                // zablokov�n� komponenty AStarCount
                enabled = false;
            }

            // nastaven� aktu�ln�ho sn�mku
            SetSprite();
        }
    }

    // aa�ten� sprit� animace
    private void LoadSpriteSheet()
    {
        string path;

        // ur�en� cesty podle aktu�ln� sc�ny
        if (SceneManager.GetActiveScene().name == "levels menu")
        {
            // n�zev rodi�e
            string parentsName = transform.parent.gameObject.name;

            // na�ten� po�tu hv�zd pro dan� level
            int stars = PlayerPrefs.GetInt("Level" + parentsName + "Stars");

            // vytvo�en� cesty k sprit�m menu
            path = stars + "starsmenu";
        }
        else
        {
            // na�ten� po�tu aktu�ln�ch hv�zd
            path = PlayerPrefs.GetInt("CurrentStars") + "gameover";
        }

        // na�ten� sprit� podle cesty
        sprites = Resources.LoadAll<Sprite>("Sprites/" + path);

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
            // astaven� sn�mku pro zobrazen�
            image.sprite = sprites[currentFrame];
        }
    }
}
