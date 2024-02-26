using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static float noteHeight;
    private static float noteWidth;
    public Note notePrefab;
    public float lastSpawnedY;
    public Note lastSpawned;
    public static Vector3 noteLocalScale;
    private float noteSpawnStartPosX;
    public static int currentNote = 0;
    public static float lastSpawnTime;

    public static GameControl Instance { get; private set; }

    private int lastNoteId = 0;
    public int LastPlayedNoteId { get; set; }

    public static List<float> spawns = new List<float>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetDataForNoteGeneration();
    }

    void FixedUpdate()
    {
        if (Time.time == lastSpawnTime + MidiFileInfo.shortestNoteSec)
            lastSpawnTime = Time.time;
            SpawnNotes();
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

        float leftSpawn = -noteWidth * 3 / 2;
        float leftMiddleSpawn = -noteWidth / 2;
        float rightMiddleSpawn = noteWidth / 2;
        float rightSpawn = noteWidth * 3 / 2;
        spawns.Add(leftMiddleSpawn);
        spawns.Add(rightMiddleSpawn);
        spawns.Add(leftSpawn);
        spawns.Add(rightSpawn);

        lastSpawnedY = spawnHeight;


    }

    private void SpawnNotes()
    {
        if (lastSpawned !=  null)
        {
            lastSpawnedY = lastSpawned.transform.position.y;
        }

        if (currentNote < MidiFileInfo.timeStamps.Count)
        {
            if (Mathf.Abs(Time.time - MidiFileInfo.timeStamps[currentNote]) < 0.01f)
            {
                int rnd = Random.Range(0, spawns.Count);

                while (lastNoteId == rnd)
                {
                    rnd = Random.Range(0, spawns.Count);
                }

                for (int i = 0; i < spawns.Count; i++)
                {
                    if (i == rnd)
                    {
                        lastSpawned = Instantiate(notePrefab, new Vector2(spawns[i], lastSpawnedY), Quaternion.identity);
                        lastSpawned.visible = true;
                    }

                    else Instantiate(notePrefab, new Vector2(spawns[i], lastSpawnedY), Quaternion.identity);
                }

                
                lastNoteId = rnd;
                currentNote++;
            }

            else foreach (float spawnPosition in spawns)
            {
                lastSpawned = Instantiate(notePrefab, new Vector2(spawnPosition, lastSpawnedY), Quaternion.identity);
            }
        }
    }
}