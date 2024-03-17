using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System.Linq;

public class Note : MonoBehaviour
{
    // components
    private Rigidbody2D rb;
    private Renderer rendererr;
    private BoxCollider2D boxcollider;
    public AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    // booly na zaznamenni doteku a viditelnosti
    public bool touched = false;
    public bool visible = false;

    // poradi rady not
    public int rowNumber;

    void Start()
    {
        // nastaveni zvuku noty
        AudioSource();

        // spawn info
        touched = false;
        rb = GetComponent<Rigidbody2D>();
        rendererr = GetComponent<Renderer>();
        boxcollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // rychlost noty
        rb.velocity = new Vector2(0f, -MidiFileInfo.speed);

        // zneviditelneni noty
        if (!visible)
        {
            rendererr.enabled = false;
        }
    }

    private void Update()
    {
        // gameobject mimo obrazovku
        if (transform.position.y <= Camera.main.ScreenToWorldPoint(Vector2.zero).y - GameControl.noteHeight) 
        {
            Destroy(gameObject);

            // pokud je nota posledni, konec hry
            if (visible) GameControl.notesPassed++;
            if (GameControl.notesPassed == MidiFileInfo.timeStamps.Count)
            {
                GameControl.Instance.StopGame();
            }
        }

        // netrefna viditelna nota mimo obrazovku
        if (transform.position.y <= (-GameControl.noteHeight * 5/ 2) && touched == false)
        {
            if (visible) 
            {
                WrongNote();
            }
        }

        // zastaveni pohybu skrz bool moving - StopGame (GameControl script)
        if (GameControl.moving == false)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    public void Hit()
    {
        // if zajisti, ze noty v zahranem radku uz nejdou odkliknout
        if (rowNumber > GameControl.currentRowNumber)
        {
            // podle viditelnosti
            if (!visible) WrongNote();
            else CorrectNote();
        }
    }

    private void WrongNote()
    {
        // zvuk spatne noty
        audioSource.clip = Resources.Load<AudioClip>("Piano/27");
        audioSource.Play();

        // zmena barvy noty + zviditelneni
        spriteRenderer.color = Color.black;
        rendererr.enabled = true;

        // zastaveni hry
        GameControl.Instance.StopGame();
    }

    private void CorrectNote()
    {
        // nota nemuze pricist body vicekrat
        if (!touched)
        {
            // zvuk noty
            audioSource.Play();

            // vypnuti (aby se v rychlejsich levelech zvuk neprekryval)
            if (rowNumber != MidiFileInfo.timeStamps.Count - 1)
            {
                StartCoroutine(TurnOffAfterDelay());
            }

            // zmena barvy
            spriteRenderer.color = Color.grey;

            // score++
            Scoreboard.Instance.ScoreUp();

            // prechod na dalsi radek
            GameControl.currentRowNumber = rowNumber;
            touched = true;
        }
    }

    
    private void AudioSource()
    {
        // nacteni audio sourcu
        audioSource = GetComponent<AudioSource>();

        // prideleni mp3 zvuku k dane note
        int noteNumber = MidiFileInfo.notes[rowNumber].NoteNumber;
        audioSource.clip = Resources.Load<AudioClip>("Piano/" + noteNumber);

    }

    private IEnumerator TurnOffAfterDelay()
    {
        yield return new WaitForSeconds(MidiFileInfo.shortestNoteSec + 0.1f);
        audioSource.Stop();
    }
}

