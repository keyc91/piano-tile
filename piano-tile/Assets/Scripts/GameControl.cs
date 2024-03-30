using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;

    // přepisováno ze scriptu NoteResize
    public static float noteHeight;
    public static List<float> spawns = new List<float>();

    // využíváno Note scriptem
    public static bool moving;
    public static int currentRowNumber; // zabraňuje řádku více než jedno kliknutí


    // kvůli instatiate
    public Note notePrefab;

    // spawn y pozice
    private Note lastSpawned;
    private Note lastSpawnedVisible;
    private float lastSpawnedY;

    // změna pozice x oproti minulé notě
    private int lastNoteId;

    public static int currentNote; // spawnování not
    private float epsilon = 0.02f; // maximální rozdíl při porovnávání časových hodnot (frekvence volání fixed updatu)
    private float spawnNotesDifference;

    // čas poslední generace noty
    private float spawnCallTime;

    private void Start()
    {
        Instance = this;

        // restart levelu
        TouchManager.allowTouchInput = true;
        currentRowNumber = -1;
        lastNoteId = -1;
        currentNote = 0;
        moving = true;

        spawnNotesDifference = (MidiFileInfo.shortestNoteSec * 2) - 0.05f;
    }

    void FixedUpdate()
    {
        // 
        float time = Time.timeSinceLevelLoad;
        float timeDifference = time - spawnCallTime;
        if (Mathf.Abs(timeDifference) < epsilon)
        {
            Debug.Log("time difference: " + timeDifference);
            SpawnNotes();
        }
    }


    private void SpawnNotes()
    {
        // kontrola, že nejsme na konci midi souboru
        if (currentNote < MidiFileInfo.timeStamps.Count)
        {
            // odpovídá čas timestampu jedné z not midi souboru?
            if (Mathf.Abs(spawnCallTime - MidiFileInfo.timeStamps[currentNote]) < spawnNotesDifference)
            {
                Debug.Log("Spawned: " + spawnCallTime + ", timestamp: " + MidiFileInfo.timeStamps[currentNote]);
                // změna generované x souřadnice oproti poslední notě
                int rnd = Random.Range(0, spawns.Count);
                while (lastNoteId == rnd)
                {
                    rnd = Random.Range(0, spawns.Count);
                }
                
                lastSpawnedY = lastSpawned?.transform?.position.y ?? NoteResize.spawnHeight;

                // čtyři noty na řádek, jedna viditelná
                for (int i = 0; i < spawns.Count; i++)
                {
                    lastSpawned = Instantiate(notePrefab, new Vector2(spawns[i], lastSpawnedY + noteHeight - (epsilon * MidiFileInfo.speed)), Quaternion.identity);

                    if (i == rnd)
                    {
                        lastSpawned.visible = true;

                        if (currentNote > 0 && lastSpawnedVisible != null)
                        {
                            Note noteComponent = lastSpawned.GetComponent<Note>();
                            noteComponent.lastAudioSource = lastSpawnedVisible.GetComponent<AudioSource>();
                            lastSpawnedVisible = lastSpawned;
                        }

                        else
                        {
                            lastSpawnedVisible = lastSpawned;
                        }
                    }

                    lastSpawned.rowNumber = currentNote;
                }

                lastNoteId = rnd;
                currentNote++;
            }

            // vytvoří čtyři noty na řádek, všechny neviditelné
            else
            {
                Debug.Log("Spawn call time: " + spawnCallTime + ", Time Stamp: " + MidiFileInfo.timeStamps[currentNote]);
                // y souřadnice posledního generovaného řádku
                lastSpawnedY = lastSpawned?.transform?.position.y ?? NoteResize.spawnHeight;

                // generace nového řádku
                foreach (float spawnPosition in spawns)
                {
                    lastSpawned = Instantiate(notePrefab, new Vector2(spawnPosition, lastSpawnedY + noteHeight - (epsilon * MidiFileInfo.speed)), Quaternion.identity);
                }
            }

            spawnCallTime = spawnCallTime + MidiFileInfo.shortestNoteSec;
        }
    }


    public void StopGame()
    {
        // zastavit všechny game objecty
        moving = false;
        TouchManager.allowTouchInput = false;

        // počet hvězd
        int stars = StarsScene();
        PlayerPrefs.SetInt("CurrentStars", stars);
        PrefEdit(stars);

        // zastavení audia
        AudioLevel.Instance.audioSource.mute = false;

        if (currentRowNumber < MidiFileInfo.timeStamps.Count - 1)
        {
            AudioLevel.Instance.audioSource.clip = Resources.Load<AudioClip>("Piano/27");
            AudioLevel.Instance.audioSource.Play();
        }

        // animace a změna scény
        StartCoroutine(DelayedTransition());
    }


    IEnumerator DelayedTransition()
    {
        // počká vteřinu, spustí animaci
        yield return new WaitForSecondsRealtime(1f);
        LevelLoader.Instance.animator.SetTrigger("Scene");
        StartCoroutine(DelayedLoadScene());
    }


    IEnumerator DelayedLoadScene()
    {
        // počká vteřinu, načte scénu 'game over'
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }


    private void PrefEdit(int stars)
    {
        string currentLevelName = PlayerPrefs.GetString("CurrentLevel");

        // pokud už je uložena starší hodnota počtu hvězd v player prefs, porovnat a případně přepsat
        if (PlayerPrefs.HasKey("Level" + currentLevelName + "Stars"))
        {
            if (stars > PlayerPrefs.GetInt("Level" + currentLevelName + "Stars")) PlayerPrefs.SetInt("Level" + currentLevelName + "Stars", stars);
        }

        // pokud není, uložit novou
        else
        {
            PlayerPrefs.SetInt("Level" + currentLevelName + "Stars", stars);
        }
    }


    private int StarsScene()
    {
        // úspěšnost v procentech
        double percent = (double)Scoreboard.scorepoints / MidiFileInfo.timeStamps.Count;

        // prepočet procent na počet hvězd (po třetinách)
        if (percent == 1) return 3;
        if (2.0 / 3.0 <= percent && percent < 1) return 2;
        if (1.0 / 3.0 <= percent && percent < 2.0 / 3.0) return 1;
        else return 0;
    }
}