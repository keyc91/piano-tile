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
    public bool touched;
    public int rowNumber;
    public AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    //SpriteRenderer spriteRenderer;

    private bool isMouseClicked;
    public bool visible;

    void Start()
    {
        AudioSource();
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
        if (transform.position.y <= Camera.main.ScreenToWorldPoint(Vector2.zero).y - GameControl.noteHeight) 
        {
            Destroy(gameObject);

            if (visible) GameControl.notesPassed++;
            if (GameControl.notesPassed == MidiFileInfo.timeStamps.Count)
            {
                GameControl.Instance.StopGame();
            }
        }

        if (transform.position.y <= (-GameControl.noteHeight * 5/ 2) && touched == false && Time.timeScale != 0f)
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
        if (rowNumber > GameControl.currentRowNumber)
        {
            if (!visible) WrongNote();
            else CorrectNote();
        }
    }

    private void WrongNote()
    {
        audioSource.clip = Resources.Load<AudioClip>("Piano/27");
        audioSource.Play();
        StartCoroutine(TurnOffAfterDelay());

        spriteRenderer.color = Color.black;

        rendererr.enabled = true;
        GameControl.Instance.StopGame();
    }

    private void CorrectNote()
    {
        if (!touched)
        {
            audioSource.Play();
            StartCoroutine(TurnOffAfterDelay());

            spriteRenderer.color = Color.grey;

            Scoreboard.Instance.ScoreUp();

            GameControl.currentRowNumber = rowNumber;

            touched = true;
        }
    }

    private void AudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        int noteNumber = MidiFileInfo.notes[rowNumber].NoteNumber;
        audioSource.clip = Resources.Load<AudioClip>("Piano/" + noteNumber);

    }

    private IEnumerator TurnOffAfterDelay()
    {
        yield return new WaitForSeconds(MidiFileInfo.shortestNoteSec + 0.2f);
        audioSource.Stop();
    }
}

