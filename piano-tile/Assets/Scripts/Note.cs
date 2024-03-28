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

    // booly na zaznamen�ni doteku a viditelnosti
    public bool touched = false;
    public bool visible = false;

    // po�ad� dan� �ady not
    public int rowNumber;

    void Start()
    {
        // nastaven� zvuku noty
        AudioSource();

        // na�ten� komponent�
        rb = GetComponent<Rigidbody2D>();
        rendererr = GetComponent<Renderer>();
        boxcollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // rychlost noty
        rb.velocity = new Vector2(0f, -MidiFileInfo.speed);

        // zneviditeln�n� noty
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

            // pokud je nota posledn�, konec hry
            if (visible) GameControl.notesPassed++;
            if (GameControl.notesPassed == MidiFileInfo.timeStamps.Count)
            {
                GameControl.Instance.StopGame();
            }
        }

        // nedot�en� viditeln� nota mimo obrazovku
        if (transform.position.y <= (-GameControl.noteHeight * 5/ 2) && !touched && visible)
        {
            WrongNote();
        }

        // zastaven� pohybu skrz bool moving - StopGame (GameControl script)
        if (GameControl.moving == false)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }


    private void AudioSource()
    {
        // na�ten� zdroje zvuku game objectu
        audioSource = GetComponent<AudioSource>();

        // p�id�len� mp3 souboru k dan� not�
        int noteNumber = MidiFileInfo.notes[rowNumber].NoteNumber;
        audioSource.clip = Resources.Load<AudioClip>("Piano/" + noteNumber);

    }


    public void Hit()
    {
        // if zajist�, �e noty v ji� zahran�m ��dku z�stanou nedot�en�
        if (rowNumber > GameControl.currentRowNumber)
        {
            // podle viditelnosti
            if (!visible) WrongNote();
            else CorrectNote();
        }
    }

    private void WrongNote()
    {
        // p�ehr�n� zvuku �patn� noty

        // zm�na barvy noty + jej� zviditeln�n�
        spriteRenderer.color = Color.black;
        rendererr.enabled = true;

        // zastaven� hry
        GameControl.Instance.StopGame();
    }

    private void CorrectNote()
    {
        // nota nebyla dot�ena - jedna nota nem��e p�i��st bod v�cekrat
        if (!touched)
        {
            // zvuk noty
            audioSource.Play();

            // zastaven� zvuku noty (aby se v rychlej��ch levelech zvuk nep�ekr�val)
            if (rowNumber != MidiFileInfo.timeStamps.Count - 1)
            {
                StartCoroutine(TurnOffAfterDelay());
            }

            // zm�na barvy
            spriteRenderer.color = Color.grey;

            // zv��en� sk�re
            Scoreboard.Instance.ScoreUp();

            // p�echod na dal�� ��dek
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

