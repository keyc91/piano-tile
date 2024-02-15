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

    private bool visible;
    public bool Visible
    {
        get => visible;
        set
        {
            visible = value;
            if (!visible)
            {

            }
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(0f, -MidiFileInfo.speed);
    }
}


