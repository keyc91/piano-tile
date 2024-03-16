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

        // nacteni not do listu
        notes = midiFile.GetNotes().ToList();


        TempoMap tempoMap = midiFile.GetTempoMap();
        MetricTimeSpan firstNote = new TimeSpan(1, 0, 0, 0);

        // nejkratsi nota, k 
        firstNote = notes[0].LengthAs<MetricTimeSpan>(tempoMap);
        shortestNoteSec = (firstNote.TotalMicroseconds / 1_000_000.0f);
        Debug.Log(shortestNoteSec);

        // list s timestamps
        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes) 
        {
            float timestamp = (note.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1_000_000.0f);
            timeStamps.Add(timestamp + shortestNoteSec);
        }

        // vypocet rychlosti not
        speed = GameControl.noteHeight / (float) firstNote.TotalSeconds;

        Debug.Log("notes " + notes.Count);
    }
}
