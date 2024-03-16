using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    public static int currentLevelIndex;

    public Note notePrefab;
    public static float noteHeight;
    private static float noteWidth;
    public static bool moving;

    public Note lastSpawned;
    public float lastSpawnedY;

    public static Vector3 noteLocalScale;
    public static int currentNote;

    private int lastNoteId;
    public static float lastSpawnTime;

    public static int notesPassed;
    public static int currentRowNumber;

    public static int[] notePositionXY;
    public static List<float> spawns = new List<float>();

    private void Awake()
    {
        notesPassed = 0;
        spawns.Clear();
        lastNoteId = 0;
        moving = true;
        Instance = this;
        currentRowNumber = -1;
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel");
    }


    void Start()
    {
        SetDataForNoteGeneration();
        currentNote = 0;
    }

    void Update()
    {
        float currentTime = (float)Time.timeSinceLevelLoad;
        if (Mathf.Abs(currentTime - (lastSpawnTime + MidiFileInfo.shortestNoteSec)) < 0.1f)
        {
            lastSpawnTime = currentTime;
            SpawnNotes();
        }
    }


    public void SetDataForNoteGeneration()
    {
        // pøizpùsobení velikosti na specifickou obrazovku
        var topRight = new Vector2(Screen.width, Screen.height);
        var topRightWorldPoint = Camera.main.ScreenToWorldPoint(topRight);
        var bottomLeftWorldPoint = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
        var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;

        // vyska a delka not
        noteHeight = screenHeight / 4;
        noteWidth = screenWidth / 4;

        var noteSpriteRenderer = notePrefab.GetComponent<SpriteRenderer>();

        // zmena velikosti prefabu noty
        notePrefab.transform.localScale = new Vector3(
               noteWidth / noteSpriteRenderer.bounds.size.x * noteSpriteRenderer.transform.localScale.x,
               noteHeight / noteSpriteRenderer.bounds.size.y * noteSpriteRenderer.transform.localScale.y, 1);

        var spawnHeight = (topRightWorldPoint.y * 5) / 4;

        // spawn positions x axis
        float leftSpawn = -noteWidth * 3 / 2;
        float leftMiddleSpawn = -noteWidth / 2;
        float rightMiddleSpawn = noteWidth / 2;
        float rightSpawn = noteWidth * 3 / 2;
        spawns.Add(leftMiddleSpawn);
        spawns.Add(rightMiddleSpawn);
        spawns.Add(leftSpawn);
        spawns.Add(rightSpawn);

        lastSpawnedY = spawnHeight;
        lastSpawnTime = -MidiFileInfo.shortestNoteSec;
    }





    private void SpawnNotes()
    {
        // y souradnice aby noty navazovaly
        if (lastSpawned != null)
        {
            lastSpawnedY = lastSpawned.transform.position.y;
        }

        if (currentNote < MidiFileInfo.timeStamps.Count)
        {
            if (Mathf.Abs(lastSpawnTime - MidiFileInfo.timeStamps[currentNote]) < 0.1f)
            {
                int rnd = Random.Range(0, spawns.Count);

                // zmena spawn x pozice oproti poslednim note
                while (lastNoteId == rnd)
                {
                    rnd = Random.Range(0, spawns.Count);
                }

                // ctyri noty na radek, jedna visible
                for (int i = 0; i < spawns.Count; i++)
                {
                    if (i == rnd)
                    {
                        lastSpawned = Instantiate(notePrefab, new Vector2(spawns[i], lastSpawnedY + noteHeight), Quaternion.identity);
                        lastSpawned.visible = true;
                    }

                    else
                    {
                        lastSpawned = Instantiate(notePrefab, new Vector2(spawns[i], lastSpawnedY + noteHeight), Quaternion.identity);
                    }

                    lastSpawned.rowNumber = currentNote;
                }

                lastSpawnTime = MidiFileInfo.timeStamps[currentNote];
                lastNoteId = rnd;
                currentNote++;
            }

            // ctyri noty na radek, vsechny invisible
            else foreach (float spawnPosition in spawns)
                {
                    lastSpawned = Instantiate(notePrefab, new Vector2(spawnPosition, lastSpawnedY + noteHeight), Quaternion.identity);
                    //Debug.Log("invisible");
                }
        }
    }





    public void StopGame()
    {
        moving = false;
        TouchManager.Instance.allowTouchInput = false;
        int scene = StarsScene();
        PlayerPrefs.SetInt("CurrentStars", scene);

        if (PlayerPrefs.HasKey("Level" + currentLevelIndex + "Stars"))
        {
            if (scene > PlayerPrefs.GetInt("Level" + currentLevelIndex + "Stars")) PlayerPrefs.SetInt("Level" + currentLevelIndex + "Stars", scene);
        }

        else
        {
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "Stars", scene);
        }

        StartCoroutine(DelayedTransition());
    }

    IEnumerator DelayedTransition()
    {
        yield return new WaitForSecondsRealtime(1f);
        LevelLoader.Instance.animator.SetTrigger("Scene");
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }





    private int StarsScene()
    {
        double percent = (double)Scoreboard.scorepoints / MidiFileInfo.timeStamps.Count;

        if (percent == 1) return 3;
        if (2.0 / 3.0 <= percent && percent < 1) return 2;
        if (1.0 / 4.0 <= percent && percent < 2.0 / 3.0) return 1;
        else return 1;
    }
}