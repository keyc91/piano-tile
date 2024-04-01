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

    // booly na zaznamenáni doteku a viditelnosti
    public bool touched = false;
    public bool visible = false;

    // poøadí dané øady not
    public int rowNumber;

    void Start()
    {
        // naètení komponentù
        audioSource = GetComponent<AudioSource>();
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

        else AudioSource();
    }

    private void Update()
    {
        // zastavení pohybu skrz bool moving - StopGame (GameControl script)
        if (GameControl.moving == false)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        
        // nedotèená viditelná nota mimo obrazovku
        if (transform.position.y <= (-GameControl.noteHeight * 5 / 2) && !touched && visible)
        {
            WrongNote();
        }

        // smazání noty mimo obrazovku
        if (transform.position.y <= Camera.main.ScreenToWorldPoint(Vector2.zero).y - (2 * GameControl.noteHeight)) 
        {
            // ztišení audia
            audioSource.mute = true;

            // destroy
            Destroy(gameObject);
        }
    }


    private void AudioSource()
    {
        // pøidìlení mp3 souboru k dané notì
        int noteNumber = MidiFileInfo.noteNumber[rowNumber];
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

            // zastavení zvuku minulé noty (aby se v rychlejších levelech zvuk nepøekrýval)
            if (lastAudioSource != null)
            {
                lastAudioSource.mute = true;
            }

            // zmìna barvy
            spriteRenderer.color = Color.grey;

            // zvýšení skóre
            Scoreboard.Instance.ScoreUp();

            // pøechod na další øádek
            GameControl.currentRowNumber = rowNumber;
            touched = true;

            // ukonèení hry pøi dosažení poslední noty
            if (rowNumber == MidiFileInfo.timeStamps.Count - 1)
            {
                GameControl.Instance.StopGame();
            }
        }
    }
}

