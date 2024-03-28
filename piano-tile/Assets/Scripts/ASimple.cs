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
                if (looping == true) 
                { 
                    image.sprite = sprites[0];
                    currentFrame = 0;
                }

                else { enabled = false; }
            }

            SetSprite();
        }
    }

    private void LoadSpriteSheet()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/" + gameObject.name);

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
