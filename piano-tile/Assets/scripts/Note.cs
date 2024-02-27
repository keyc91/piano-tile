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
    //SpriteRenderer spriteRenderer;

    private bool isMouseClicked;
    public bool visible = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rendererr = GetComponent<Renderer>();
        boxcollider = GetComponent<BoxCollider2D>();

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
        }
        // smaze notu mimo obrazovku
    }

    public void Hit()
    {
        if (!visible) WrongNote();
        else CorrectNote();
    }

    private void WrongNote()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.black;

        rendererr.enabled = true;
    }

    private void CorrectNote()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.grey;
    }
}


