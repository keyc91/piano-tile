using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static float noteHeight;
    private static float noteWidth;
    public Note notePrefab;
    public static Vector3 noteLocalScale;
    private float noteSpawnStartPosX;
    public Transform lastSpawnedNote;
    public static int currentNote;

    public static GameControl Instance { get; private set; }
    public ReactiveProperty<bool> GameStarted { get; set; }
    public ReactiveProperty<bool> GameOver { get; set; }
    public ReactiveProperty<int> Score { get; set; }

    private int lastNoteId = 1;
    public int LastPlayedNoteId { get; set; }

    public static List<Vector2> spawns = new List<Vector2>();

    private void Awake()
    {
        Instance = this;
        GameStarted = new ReactiveProperty<bool>();
        GameOver = new ReactiveProperty<bool>();
        Score = new ReactiveProperty<int>();
    }

    void Start()
    {
        SetDataForNoteGeneration();
        SpawnNotes();
    }

    void Update()
    {
        const float epsilon = 0.01f; // Adjust as needed
        if (Mathf.Abs(Time.time - MidiFileInfo.timeStamps[currentNote]) < epsilon)
        {
            SpawnNotes();
        }
    }

    public void SetDataForNoteGeneration()
    {
        var topRight = new Vector2(Screen.width, Screen.height);
        var topRightWorldPoint = Camera.main.ScreenToWorldPoint(topRight);
        var bottomLeftWorldPoint = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
        var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;
        // pøizpùsobení velikosti na specifickou obrazovku

        noteHeight = screenHeight / 4;
        noteWidth = screenWidth / 4;
        // velikost not

        var noteSpriteRenderer = notePrefab.GetComponent<SpriteRenderer>();

        notePrefab.transform.localScale = new Vector3(
               noteWidth / noteSpriteRenderer.bounds.size.x * noteSpriteRenderer.transform.localScale.x,
               noteHeight / noteSpriteRenderer.bounds.size.y * noteSpriteRenderer.transform.localScale.y, 1);

        var spawnHeight = (topRightWorldPoint.y * 5) / 4;

        Vector2 leftSpawn = new Vector2(-(noteWidth * 3 / 2), spawnHeight);
        Vector2 leftMiddleSpawn = new Vector2(-noteWidth / 2, spawnHeight);
        Vector2 rightMiddleSpawn = new Vector2(noteWidth / 2, spawnHeight);
        Vector2 rightSpawn = new Vector2((noteWidth * 3) / 2, spawnHeight);
        spawns.Add(leftMiddleSpawn);
        spawns.Add(rightMiddleSpawn);
        spawns.Add(leftSpawn);
        spawns.Add(rightSpawn);
    }

    private void SpawnNotes()
    {
        foreach(Vector2 spawnPosition in spawns)
            Instantiate(notePrefab, spawnPosition, Quaternion.identity);

        currentNote++;
        Debug.Log(currentNote);
    }
}
