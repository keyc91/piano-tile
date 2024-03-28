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
        currentFrame = 0;
        image = GetComponent<Image>();
        LoadSpriteSheet();
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad - (timePerFrame) >= lastCallTime)
        {
            lastCallTime = Time.timeSinceLevelLoad;
            currentFrame++;

            if (currentFrame >= sprites.Length)
            {
                enabled = false;
            }

            SetSprite();
        }
    }

    private void LoadSpriteSheet()
    {
        string path;
        if (SceneManager.GetActiveScene().name == "levels menu")
        {
            path = PlayerPrefs.GetInt("CurrentStars") + "starsmenu";
        }

        else
        {
            path = PlayerPrefs.GetInt("CurrentStars") + "gameover";
        }

        sprites = Resources.LoadAll<Sprite>("Sprites/" + path);

        timePerFrame = 1f / frameRate;

        SetSprite();
    }

    private void SetSprite()
    {
        if (currentFrame >= 0 && currentFrame < sprites.Length)
        {
            image.sprite = sprites[currentFrame];
        }
    }
}
