/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System;
using System.Linq;
using UnityEngine.Networking;


public class MidiFileInfo2 : MonoBehaviour
{
    public static MidiFileInfo2 Instance;
    public MidiFile midiFile;
    public static float speed;
    public static List<float> timeStamps = new List<float>();
    public static List<Melanchall.DryWetMidi.Interaction.Note> notes = new List<Melanchall.DryWetMidi.Interaction.Note>();
    public static float shortestNoteSec;

    private void Awake()
    {
        Instance = this;

        // restart scÈny
        notes.Clear();
        timeStamps.Clear();

        Debug.Log("Awake midifileinfo");
        StartCoroutine(LoadFile(PlayerPrefs.GetString("CurrentLevel")));
    }

    private void ProcessMidiFile()
    {
        // jmÈno levelu
        string scene = PlayerPrefs.GetString("CurrentLevel");

        string destinationFilePath = Path.Combine(Application.dataPath, "Midi/" + scene + ".mid");
        midiFile = MidiFile.Read(destinationFilePath);

        // p¯epis bpm pokud je definov·no, jinak v˝chozÌ hodnota 120
        long bpm = bpms.ContainsKey(scene) ? bpms[scene] : 120;

        // hodnota ËtvrùovÈ noty v mikrovte¯in·ch
        long microsecondsPerQuarterNote = (long)60000000.0 / bpm;

        // zmÏna tempa midi souboru
        using (var tempoMapManager = midiFile.ManageTempoMap())
        {
            tempoMapManager.SetTempo(new MetricTimeSpan(0), new Tempo(microsecondsPerQuarterNote));
        }

        // naËtenÌ not do seznamu
        notes = midiFile.GetNotes().ToList();
        Debug.Log("poËet not: " + notes.Count);

        // hodnota nejkratöÌ noty ve vte¯in·ch
        TempoMap tempoMap = midiFile.GetTempoMap();
        MetricTimeSpan firstNote = notes[0].LengthAs<MetricTimeSpan>(tempoMap);
        shortestNoteSec = (float)firstNote.TotalSeconds;
        Debug.Log("DÈlka nejkratöÌ noty: " + shortestNoteSec);

        // tempo midi souboru (kontrola)
        Tempo tempo = tempoMap.GetTempoAtTime(firstNote);
        double bpmnow = 60000000.0 / tempo.MicrosecondsPerQuarterNote;
        Debug.Log("Tempo: " + bpmnow + " BPM");

        // naËtenÌ Ëasu generace not do seznamu (vte¯iny)
        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
        {
            float timestamp = (note.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1_000_000.0f);

            // + Ëasov· rezerva jedna Ëtvrtov· nota
            timeStamps.Add(timestamp + shortestNoteSec);
        }

        // v˝poËet rychlosti not
        speed = GameControl.noteHeight / (float)firstNote.TotalSeconds;
    }


    IEnumerator LoadFile(string scene)
    {
        // Path to the text file in StreamingAssets folder
        string sourceFilePath = Path.Combine(Application.streamingAssetsPath, scene + ".mid");
        Debug.Log("source: " + sourceFilePath);

        // Path to the destination folder
        string destinationFolderPath = Path.Combine(Application.dataPath, "Midi");
        Debug.Log("folder: " + destinationFolderPath);

        string destinationFilePath = Path.Combine(destinationFolderPath, scene + ".mid");
        Debug.Log("file path: " + destinationFilePath);

        // Check if the file exists in the persistent data path
        if (!File.Exists(destinationFilePath))
        {
            // Create a UnityWebRequest to read the file data
            UnityWebRequest www = UnityWebRequest.Get(sourceFilePath);

            // Send the request
            yield return www.SendWebRequest();

            // Check if the request was successful
            if (!www.isNetworkError && !www.isHttpError)
            {
                // Save the downloaded asset to the persistent data path
                File.WriteAllBytes(destinationFilePath, www.downloadHandler.data);

                ProcessMidiFile();

                Debug.Log("Asset copied successfully.");
            }
            else
            {
                Debug.LogError("Failed to copy asset: " + www.error);
            }
        }
        else
        {
            Debug.Log("Asset already exists in the persistent data path.");
            ProcessMidiFile();
        }
    }

    // slovnÌk s bpm hodnotou kaûdÈho levelu
    private Dictionary<string, long> bpms = new Dictionary<string, long>()
    {
        { "CDur", 120 },
        { "Hafo", 105 },
        { "NeverGonna", 110 },
        { "BlueDanube", 135 },
        { "Canon", 90 },
        { "FurElise", 80 },
        { "RiverFlows", 120 },
        { "Tequila ", 120 },
        { "TwinkleTwinkle", 120 }
    };
}*/