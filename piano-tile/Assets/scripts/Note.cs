using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System.Linq;

public class Note : MonoBehaviour
{
    private Rigidbody2D rb;
    private Renderer rendererr;
    private BoxCollider2D boxcollider;
    private bool touched;
    private SpriteRenderer spriteRenderer;
    //SpriteRenderer spriteRenderer;

    private bool isMouseClicked;
    public bool visible;

    void Start()
    {
        touched = false;
        rb = GetComponent<Rigidbody2D>();
        rendererr = GetComponent<Renderer>();
        boxcollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.velocity = new Vector2(0f, -MidiFileInfo.speed);
        // nastavi rychlost noty

        if (!visible)
        {
            rendererr.enabled = false;
        }
        // zneviditelni noty

    }

    private void Update()
    {
        if (transform.position.y <= Camera.main.ScreenToWorldPoint(Vector2.zero).y - GameControl.noteHeight/2) 
        {
            Destroy(gameObject);
            if (visible) GameControl.notesPassed++;
            Debug.Log("passed " + GameControl.notesPassed);
            Debug.Log("all notes " + MidiFileInfo.timeStamps.Count);
            if (GameControl.notesPassed == MidiFileInfo.timeStamps.Count)
            {
                Debug.Log("stop game");
                GameControl.Instance.StopGame();
            }
        }

        if (transform.position.y <= (-GameControl.noteHeight * 3 / 2) && touched == false && Time.timeScale != 0f)
        {
            if (visible) 
            {
                WrongNote();
            }
        }

        if (GameControl.moving == false)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    public void Hit()
    {
        if (!visible) WrongNote();
        else CorrectNote(); 
    }

    private void WrongNote()
    {
        spriteRenderer.color = Color.black;

        rendererr.enabled = true;
        GameControl.Instance.StopGame();
    }

    private void CorrectNote()
    {
        if (!touched)
        {
            spriteRenderer.color = Color.grey;

            Scoreboard.Instance.ScoreUp();

            touched = true;
        }
    }
}


