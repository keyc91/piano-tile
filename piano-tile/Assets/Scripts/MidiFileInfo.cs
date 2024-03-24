using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System;
using System.Linq;

public class MidiFileInfo : MonoBehaviour
{
    public static MidiFileInfo Instance;
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

        LoadMidiFile();
    }

    private void LoadMidiFile()
    {
        // jmÈno levelu
        string scene = PlayerPrefs.GetString("CurrentLevel");

        // naËtenÌ midi souboru pomoci jmÈna levelu
        string filePath = Path.Combine(Application.dataPath, "Resources/Midi/" + scene + ".mid");
        midiFile = MidiFile.Read(filePath);

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

    // slovnÌk s bpm hodnotou kaûdÈho levelu
    private Dictionary<string, long> bpms = new Dictionary<string, long>()
    {
        { "CDur", 120 },
        { "Hafo", 105 },
        { "NeverGonna", 110 },
        { "BlueDanube", 135 },
        { "Canon", 105 },
        { "FurElise", 80 },
        { "RiverFlows", 120 },
        { "Tequila ", 120 },
        { "TwinkleTwinkle", 120 }
    };
}