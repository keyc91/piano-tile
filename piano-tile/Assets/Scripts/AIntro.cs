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
                if (image.sprite.name == "intro_19")
                {
                    sprites = new Sprite[0];
                    sprites = Resources.LoadAll<Sprite>("Sprites/IntroLoop");
                    currentFrame = 0;
                }

                else
                {
                    image.sprite = sprites[0];
                    currentFrame = 0;
                }
            }

            SetSprite();
        }
    }

    private void LoadSpriteSheet()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Intro");

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
