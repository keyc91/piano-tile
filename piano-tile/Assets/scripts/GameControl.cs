using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    private static float noteHeight;
    private static float noteWidth;
    public Note notePrefab;
    private Vector3 noteLocalScale;
    private float noteSpawnStartPosX;
    public Transform lastSpawnedNote;


    public static GameControl Instance { get; private set; }
    public ReactiveProperty<bool> GameStarted { get; set; }
    public ReactiveProperty<bool> GameOver { get; set; }
    public ReactiveProperty<int> Score { get; set; }

    private void Awake()
    {
        Instance = this;
        GameStarted = new ReactiveProperty<bool>();
        GameOver = new ReactiveProperty<bool>();
        Score = new ReactiveProperty<int>();
        ShowGameOverScreen = new ReactiveProperty<bool>();
    }

    void Start()
    {
        SetDataForNoteGeneration();
    }

    void Update()
    {
        DetectNoteClicks();
    }

    private void SetDataForNoteGeneration()
    {
        var topRight = new Vector3(Screen.width, Screen.height, 0);
        var topRightWorldPoint = Camera.main.ScreenToWorldPoint(topRight);
        var bottomLeftWorldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
        var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;
        // pøizpùsobení velikosti na specifickou obrazovku

        noteHeight = screenHeight / 4;
        noteWidth = screenWidth / 4;
        // velikost not

        noteLocalScale = new Vector3(
               noteWidth / noteSpriteRenderer.bounds.size.x * noteSpriteRenderer.transform.localScale.x,
               noteHeight / noteSpriteRenderer.bounds.size.y * noteSpriteRenderer.transform.localScale.y, 1);

        var leftmostPoint = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height / 2));
        var leftmostPointPivot = leftmostPoint.x + noteWidth / 2;
        noteSpawnStartPosX = leftmostPointPivot;
    }

    private void DetectNoteClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(origin, Vector2.zero);
            if (hit)
            {
                var gameObject = hit.collider.gameObject;
                if (gameObject.CompareTag("Note"))
                {
                    var note = gameObject.GetComponent<Note>();
                    note.Play();
                }
            }
        }
    }

    /*public void SpawnNotes()
    {
        if (lastSpawn) return;

        var noteSpawnStartPosY = lastSpawnedNote.position.y + noteHeight;
        Note note = null;
        var timeTillEnd = audioSource.clip.length - audioSource.time;
        int notesToSpawn = NotesToSpawn;
        if (timeTillEnd < NotesToSpawn)
        {
            notesToSpawn = Mathf.CeilToInt(timeTillEnd);
            lastSpawn = true;
        }
        for (int i = 0; i < notesToSpawn; i++)
        {
            var randomIndex = GetRandomIndex();
            for (int j = 0; j < 4; j++)
            {
                note = Instantiate(notePrefab, noteContainer.transform);
                note.transform.localScale = noteLocalScale;
                note.transform.position = new Vector2(noteSpawnStartPosX + noteWidth * j, noteSpawnStartPosY);
                note.Visible = (j == randomIndex);
                if (note.Visible)
                {
                    note.Id = lastNoteId;
                    lastNoteId++;
                }
            }
            noteSpawnStartPosY += noteHeight;
            if (i == NotesToSpawn - 1) lastSpawnedNote = note.transform;
        }
    }

    private int GetRandomIndex()
    {
        var randomIndex = Random.Range(0, 4);
        while (randomIndex == prevRandomIndex) randomIndex = Random.Range(0, 4);
        prevRandomIndex = randomIndex;
        return randomIndex;
    }*/
}
