using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System.Linq;

public class Note : MonoBehaviour
{
    // komponenty
    private Rigidbody2D rb;
    private Renderer rendererr;
    private BoxCollider2D boxcollider;
    public AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    // booly na zaznamenáni doteku a viditelnosti
    public bool touched = false;
    public bool visible = false;

    // poøadí dané øady not
    public int rowNumber;

    void Start()
    {
        // nastavení zvuku noty
        AudioSource();

        // naètení komponentù
        rb = GetComponent<Rigidbody2D>();
        rendererr = GetComponent<Renderer>();
        boxcollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // rychlost noty
        rb.velocity = new Vector2(0f, -MidiFileInfo.speed);

        // zneviditelnìní noty
        if (!visible)
        {
            rendererr.enabled = false;
        }
    }

    private void Update()
    {
        // game object mimo obrazovku
        if (transform.position.y <= Camera.main.ScreenToWorldPoint(Vector2.zero).y - GameControl.noteHeight) 
        {
            Destroy(gameObject);

            // pokud je nota poslední, konec hry
            if (visible) GameControl.notesPassed++;
            if (GameControl.notesPassed == MidiFileInfo.timeStamps.Count)
            {
                GameControl.Instance.StopGame();
            }
        }

        // nedotèená viditelná nota mimo obrazovku
        if (transform.position.y <= (-GameControl.noteHeight * 5/ 2) && !touched && visible)
        {
            WrongNote();
        }

        // zastavení pohybu skrz bool moving - StopGame (GameControl script)
        if (GameControl.moving == false)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }


    private void AudioSource()
    {
        // naètení zdroje zvuku game objectu
        audioSource = GetComponent<AudioSource>();

        // pøidìlení mp3 souboru k dané notì
        int noteNumber = MidiFileInfo.notes[rowNumber].NoteNumber;
        audioSource.clip = Resources.Load<AudioClip>("Piano/" + noteNumber);

    }


    public void Hit()
    {
        // if zajistí, že noty v již zahraném øádku zùstanou nedotèené
        if (rowNumber > GameControl.currentRowNumber)
        {
            // podle viditelnosti
            if (!visible) WrongNote();
            else CorrectNote();
        }
    }

    private void WrongNote()
    {
        // pøehrání zvuku špatné noty

        // zmìna barvy noty + její zviditelnìní
        spriteRenderer.color = Color.black;
        rendererr.enabled = true;

        // zastavení hry
        GameControl.Instance.StopGame();
    }

    private void CorrectNote()
    {
        // nota nebyla dotèena - jedna nota nemùže pøièíst bod vícekrat
        if (!touched)
        {
            // zvuk noty
            audioSource.Play();

            // zastavení zvuku noty (aby se v rychlejších levelech zvuk nepøekrýval)
            if (rowNumber != MidiFileInfo.timeStamps.Count - 1)
            {
                StartCoroutine(TurnOffAfterDelay());
            }

            // zmìna barvy
            spriteRenderer.color = Color.grey;

            // zvýšení skóre
            Scoreboard.Instance.ScoreUp();

            // pøechod na další øádek
            GameControl.currentRowNumber = rowNumber;
            touched = true;
        }
    }

    private IEnumerator TurnOffAfterDelay()
    {
        yield return new WaitForSeconds(MidiFileInfo.shortestNoteSec + 0.2f);
        audioSource.Stop();
    }
}

