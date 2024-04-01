using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class Note : MonoBehaviour
{
    // komponenty
    private Rigidbody2D rb;
    private Renderer rendererr;
    private BoxCollider2D boxcollider;
    public AudioSource audioSource;
    public AudioSource lastAudioSource;
    private SpriteRenderer spriteRenderer;

    // booly na zaznamen�ni doteku a viditelnosti
    public bool touched = false;
    public bool visible = false;

    // po�ad� dan� �ady not
    public int rowNumber;

    void Start()
    {
        // na�ten� komponent�
        audioSource = GetComponent<AudioSource>();
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

        else AudioSource();
    }

    private void Update()
    {
        // zastaven� pohybu skrz bool moving - StopGame (GameControl script)
        if (GameControl.moving == false)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        
        // nedot�en� viditeln� nota mimo obrazovku
        if (transform.position.y <= (-GameControl.noteHeight * 5 / 2) && !touched && visible)
        {
            WrongNote();
        }

        // smaz�n� noty mimo obrazovku
        if (transform.position.y <= Camera.main.ScreenToWorldPoint(Vector2.zero).y - (2 * GameControl.noteHeight)) 
        {
            // zti�en� audia
            audioSource.mute = true;

            // destroy
            Destroy(gameObject);
        }
    }


    private void AudioSource()
    {
        // p�id�len� mp3 souboru k dan� not�
        int noteNumber = MidiFileInfo.noteNumber[rowNumber];
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

            // zastaven� zvuku minul� noty (aby se v rychlej��ch levelech zvuk nep�ekr�val)
            if (lastAudioSource != null)
            {
                lastAudioSource.mute = true;
            }

            // zm�na barvy
            spriteRenderer.color = Color.grey;

            // zv��en� sk�re
            Scoreboard.Instance.ScoreUp();

            // p�echod na dal�� ��dek
            GameControl.currentRowNumber = rowNumber;
            touched = true;

            // ukon�en� hry p�i dosa�en� posledn� noty
            if (rowNumber == MidiFileInfo.timeStamps.Count - 1)
            {
                GameControl.Instance.StopGame();
            }
        }
    }
}

