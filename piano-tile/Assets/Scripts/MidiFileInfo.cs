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
    public MidiFile midiFile;
    public static float speed;
    public static List<float> timeStamps = new List<float>();
    public static float shortestNoteSec;

    private void Start()
    {
        LoadMidiFile();
    }

    private void LoadMidiFile()
    {
        string filePath = Path.Combine(Application.dataPath, "Midi/piano.mid");
        midiFile = MidiFile.Read(filePath);

        IEnumerable<Melanchall.DryWetMidi.Interaction.Note> notes = midiFile.GetNotes();
        TempoMap tempoMap = midiFile.GetTempoMap();
        MetricTimeSpan shortestNote = new TimeSpan(1, 0, 0, 0);

        foreach (Melanchall.DryWetMidi.Interaction.Note note in notes) 
        {
            //Debug.Log($"length {note.LengthAs<MetricTimeSpan>(tempoMap)}");
            if (shortestNote > note.LengthAs<MetricTimeSpan>(tempoMap))
            {
                shortestNote = note.LengthAs<MetricTimeSpan>(tempoMap);
            }

            //Debug.Log($"name {note.NoteNumber}");
            //Debug.Log($"time {note.TimeAs<MetricTimeSpan>(tempoMap)}");

            float timestamp = (note.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1_000_000.0f);

            shortestNoteSec = (shortestNote.TotalMicroseconds / 1_000_000.0f);
            Debug.Log($"timestamp {timestamp}");
            timeStamps.Add(timestamp);
        }

        speed = GameControl.noteHeight / (float) shortestNote.TotalSeconds;
    }
}
