using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System.Linq;

public class Note : MonoBehaviour
{
    private bool isMouseClicked;
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public bool visible = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, -MidiFileInfo.speed);

        animator = GetComponent<Animator>();
        Renderer renderer = GetComponent<Renderer>();

        if (!visible)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    private void Update()
    {
        if (transform.position.y <= Camera.main.ScreenToWorldPoint(Vector2.zero).y - GameControl.noteHeight/2) 
        {
            Destroy(gameObject);
        }
    }

    /*void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)/* && color.color != Color.grey)
        {
            Debug.Log("click");
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            if (!visible)
                WrongNote();

            else if (visible)
                CorrectNote();
        }
    }*/

    private void WrongNote()
    {
        Debug.Log("wrong");
    }

    private void CorrectNote()
    {
        Debug.Log("slay");
    }
}


