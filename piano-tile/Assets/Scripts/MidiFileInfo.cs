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
        notes.Clear();
        timeStamps.Clear();
        LoadMidiFile();
    }

    private void LoadMidiFile()
    {
        // nacteni midi filu podle levelu
        string filePath = Path.Combine(Application.dataPath, "Resources/Midi/" + PlayerPrefs.GetString("CurrentLevel") + ".mid");
        midiFile = MidiFile.Read(filePath);

        // jmeno levelu
        string scene = PlayerPrefs.GetString("CurrentLevel");

        // prepsat bpm pokud definovano, jinak defaultne 120
        long bpm = bpms.ContainsKey(scene) ? bpms[scene] : 120;

        //
        long microsecondsPerQuarterNote = (long)60000000.0 / bpm;

        using (var tempoMapManager = midiFile.ManageTempoMap())
        {
            tempoMapManager.SetTempo(new MetricTimeSpan(0), new Tempo(microsecondsPerQuarterNote));
        }   

        // nacteni not do listu
        notes = midiFile.GetNotes().ToList();
        Debug.Log("notes count: " + notes.Count);

        TempoMap tempoMap = midiFile.GetTempoMap();

        // nejkratsi nota
        MetricTimeSpan firstNote = notes[0].LengthAs<MetricTimeSpan>(tempoMap);
        shortestNoteSec = (float)firstNote.TotalSeconds;
        Debug.Log("Shortest note length: " + shortestNoteSec);

        // current tempo
        Tempo tempo = tempoMap.GetTempoAtTime(firstNote);
        double bpmnow = 60000000.0 / tempo.MicrosecondsPerQuarterNote;
        Debug.Log("Tempo: " + bpmnow + " BPM");

        // list s timestamps
        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes) 
        {
            float timestamp = (note.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1_000_000.0f);
            timeStamps.Add(timestamp + shortestNoteSec);
        }

        // vypocet rychlosti not
        speed = GameControl.noteHeight / (float) firstNote.TotalSeconds;
    }

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