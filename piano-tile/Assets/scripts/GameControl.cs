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
    public static bool moving;

    public Note lastSpawned;
    public float lastSpawnedY;

    public static Vector3 noteLocalScale;
    public static int currentNote;

    private int lastNoteId;
    public static float lastSpawnTime;

    public static List<float> spawns = new List<float>();

    private void Awake()
    {
        spawns.Clear();
        lastNoteId = 0;
        moving = true;
        Instance = this;
    }


    void Start()
    {
        SetDataForNoteGeneration();
        currentNote = 0;
        Debug.Log("note 0");
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
        if (lastSpawned != null)
        {
            lastSpawnedY = lastSpawned.transform.position.y;
        }
        // y souradnice aby noty navazovaly

        if (currentNote < MidiFileInfo.timeStamps.Count)
        {
            if (Mathf.Abs(lastSpawnTime - MidiFileInfo.timeStamps[currentNote]) < 0.1f)
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
                        Debug.Log(rnd);
                    }

                    else
                    {
                        Instantiate(notePrefab, new Vector2(spawns[i], lastSpawnedY + noteHeight), Quaternion.identity);
                    }
                }
                // ctyri noty na radek, jedna visible

                lastSpawnTime = MidiFileInfo.timeStamps[currentNote];
                lastNoteId = rnd;
                currentNote++;
            }

            else foreach (float spawnPosition in spawns)
                {
                    Debug.Log("Spawning invidible");
                    lastSpawned = Instantiate(notePrefab, new Vector2(spawnPosition, lastSpawnedY + noteHeight), Quaternion.identity);
                    //Debug.Log("invisible");
                }
            // ctyri noty na radek, vsechny invisible
        }
    }





    public void StopGame()
    {
        moving = false;
        TouchManager.Instance.allowTouchInput = false;
        int scene = StarsScene();
        StartCoroutine(DelayedTransition(scene));
    }

    IEnumerator DelayedTransition(int scene)
    {
        yield return new WaitForSecondsRealtime(1f);
        LevelLoader.Instance.animator.SetTrigger("Scene");
        StartCoroutine(DelayedLoadScene(scene));
    }

    IEnumerator DelayedLoadScene(int scene)
    {
        yield return new WaitForSecondsRealtime(1f);
        LoadScene(scene);
    }

    private void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }





    private int StarsScene()
    {
        double percent = (double)MidiFileInfo.timeStamps.Count / Scoreboard.scorepoints;

        if (percent == 1) return 1;
        if (2.0 / 3.0 <= percent && percent < 1) return 1;
        if (1.0 / 3.0 <= percent && percent < 2.0 / 3.0) return 1;
        else return 1;
    }
}