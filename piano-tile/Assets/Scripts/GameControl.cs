using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    // edited z NoteResize
    public static float noteHeight;
    public static List<float> spawns = new List<float>();

    // vyuzivano Note scriptem
    public static int notesPassed; // konec hry trigger
    public static bool moving;
    public static int currentRowNumber;


    // kvuli instatiate
    public Note notePrefab;

    // spawn y pozice
    private Note lastSpawned;
    private float lastSpawnedY;

    // zmena pozice x oproti minule
    private int lastNoteId;

    public static int currentNote;
    private float epsilon;

    // cas v game control
    private float spawnCallTime;

    private void Start()
    {
        Instance = this;

        // restart levelu
        TouchManager.allowTouchInput = true;
        currentRowNumber = -1;
        lastNoteId = -1;
        Debug.Log("last note id " + lastNoteId);
        currentNote = 0;
        notesPassed = 0;
        moving = true;
        lastSpawnedY = NoteResize.spawnHeight;
        spawnCallTime = -MidiFileInfo.shortestNoteSec;

        epsilon = 0.02f;
    }

    void FixedUpdate()
    {
        // 
        float time = Time.timeSinceLevelLoad;
        float timeDifference = time - (spawnCallTime + MidiFileInfo.shortestNoteSec);

        if (Mathf.Abs(timeDifference) < epsilon)
        {
            spawnCallTime = spawnCallTime + MidiFileInfo.shortestNoteSec;
            SpawnNotes();
        }
    }


    private void SpawnNotes()
    {
        // kontrola, ze nejsme na konci midi filu
        if (currentNote < MidiFileInfo.timeStamps.Count)
        {
            // odpovida cas timestampu jedne z not midifilu?
            if (Mathf.Abs(spawnCallTime - MidiFileInfo.timeStamps[currentNote]) < epsilon)
            {
                //Debug.Log("went through spawn difference:" + (spawnCallTime - MidiFileInfo.timeStamps[currentNote]));

                // zmena spawn x pozice oproti poslednim note
                int rnd = Random.Range(0, spawns.Count);
                while (lastNoteId == rnd)
                {
                    rnd = Random.Range(0, spawns.Count);
                }

                // y souradnice posledniho radku not
                if (lastSpawned != null)
                {
                    lastSpawnedY = lastSpawned.transform.position.y;
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

                lastNoteId = rnd;
                currentNote++;
            }

            // vytvori ctyri noty na radek, vsechny neviditelne
            else
            {
                // hodnota y posledniho spawnuteho radku
                if (lastSpawned != null)
                {
                    lastSpawnedY = lastSpawned.transform.position.y;
                }

                // spawn radku
                foreach (float spawnPosition in spawns)
                {
                    lastSpawned = Instantiate(notePrefab, new Vector2(spawnPosition, lastSpawnedY + noteHeight), Quaternion.identity);
                }
            }
        }
    }


    public void StopGame()
    {
        // zastavit vsechny gameobjects
        moving = false;
        TouchManager.allowTouchInput = false;

        // pocet hvezdicek
        int stars = StarsScene();
        PlayerPrefs.SetInt("CurrentStars", stars);
        PrefEdit(stars);

        // animace a zmena sceny
        StartCoroutine(DelayedTransition());
    }


    IEnumerator DelayedTransition()
    {
        // pocka vterinu, spusti animaci
        yield return new WaitForSecondsRealtime(1f);
        LevelLoader.Instance.animator.SetTrigger("Scene");
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        // pocka vterinu, nacte scene game over
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }


    private void PrefEdit(int stars)
    {
        string currentLevelName = PlayerPrefs.GetString("CurrentLevel");

        // pokud uz je ulozena starsi hodnota poctu hvezd v player prefs, porovnat a pripadne prepsat
        if (PlayerPrefs.HasKey("Level" + currentLevelName + "Stars"))
        {
            if (stars > PlayerPrefs.GetInt("Level" + currentLevelName + "Stars")) PlayerPrefs.SetInt("Level" + currentLevelName + "Stars", stars);
        }

        // pokud neni, ulozit novou
        else
        {
            PlayerPrefs.SetInt("Level" + currentLevelName + "Stars", stars);
        }
    }

    private int StarsScene()
    {
        // uspesnost v procentech
        double percent = (double)Scoreboard.scorepoints / MidiFileInfo.timeStamps.Count;

        // prepocet procent na pocet hvezd (po tretinach)
        if (percent == 1) return 3;
        if (2.0 / 3.0 <= percent && percent < 1) return 2;
        if (1.0 / 3.0 <= percent && percent < 2.0 / 3.0) return 1;
        else return 0;
    }
}