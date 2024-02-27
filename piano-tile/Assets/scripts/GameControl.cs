using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    public Note notePrefab;
    public static float noteHeight;
    private static float noteWidth;

    public Note lastSpawned;
    public float lastSpawnedY;

    public static Vector3 noteLocalScale;
    public static int currentNote = 0;

    private int lastNoteId = 0;
    public static float lastSpawnTime;

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
        if (Mathf.Approximately(Time.time, lastSpawnTime + MidiFileInfo.shortestNoteSec))
        {
            lastSpawnTime = Time.time;
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
        // vyska a delka not

        var noteSpriteRenderer = notePrefab.GetComponent<SpriteRenderer>();

        notePrefab.transform.localScale = new Vector3(
               noteWidth / noteSpriteRenderer.bounds.size.x * noteSpriteRenderer.transform.localScale.x,
               noteHeight / noteSpriteRenderer.bounds.size.y * noteSpriteRenderer.transform.localScale.y, 1);
        // zmena velikosti prefabu noty

        var spawnHeight = (topRightWorldPoint.y * 5) / 4;

        float leftSpawn = -noteWidth * 3 / 2;
        float leftMiddleSpawn = -noteWidth / 2;
        float rightMiddleSpawn = noteWidth / 2;
        float rightSpawn = noteWidth * 3 / 2;
        spawns.Add(leftMiddleSpawn);
        spawns.Add(rightMiddleSpawn);
        spawns.Add(leftSpawn);
        spawns.Add(rightSpawn);
        // spawn positions x axis

        lastSpawnedY = spawnHeight;
        lastSpawnTime = -MidiFileInfo.shortestNoteSec;

    }

    private void SpawnNotes()
    {
        if (lastSpawned !=  null)
        {
            lastSpawnedY = lastSpawned.transform.position.y;
        }
        // y souradnice aby noty navazovaly

        if (currentNote < MidiFileInfo.timeStamps.Count)
        {
            if (Mathf.Approximately(lastSpawnTime, MidiFileInfo.timeStamps[currentNote]))
            {
                int rnd = Random.Range(0, spawns.Count);

                while (lastNoteId == rnd)
                {
                    rnd = Random.Range(0, spawns.Count);
                }
                // zmena spawn x pozice oproti poslednim note

                for (int i = 0; i < spawns.Count; i++)
                {
                    if (i == rnd)
                    {
                        lastSpawned = Instantiate(notePrefab, new Vector2(spawns[i], lastSpawnedY + noteHeight), Quaternion.identity);
                        lastSpawned.visible = true;
                        //Debug.Log("visible note");
                    }

                    else
                    {
                        Instantiate(notePrefab, new Vector2(spawns[i], lastSpawnedY + noteHeight), Quaternion.identity);
                        //Debug.Log("invisible buddies");
                    }
                }
                // ctyri noty na radek, jedna visible
                
                lastNoteId = rnd;
                currentNote++;
            }

            else foreach (float spawnPosition in spawns)
            {
                lastSpawned = Instantiate(notePrefab, new Vector2(spawnPosition, lastSpawnedY + noteHeight), Quaternion.identity);
                    //Debug.Log("invisible");
            }
            // ctyri noty na radek, vsechny invisible
        }
    }

    public void StopGame()
    {
        Time.timeScale = 0;
        Debug.Log("Game stopped");
        TouchManager.Instance.allowTouchInput = false;
    }
}