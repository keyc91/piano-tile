using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System.Linq;

public class Note : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public bool visible = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (visible == false)
        { 
            Renderer renderer = GetComponent<Renderer>();
            GetComponent<Renderer>().enabled = false;
        }
    }

    private void Update()
    {
        rb.velocity = new Vector2(0f, -MidiFileInfo.speed);
        if (transform.position.y <= Camera.main.ScreenToWorldPoint(Vector2.zero).y - GameControl.noteHeight/2) 
        {
            Destroy(gameObject);
        }
    }
}


