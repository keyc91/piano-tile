using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MidiToText : MonoBehaviour
{
    // info od uivatele
    public new string name;
    public long bpm;

    // cesta k textovému souboru
    private string textPath;

    // midifile
    private MidiFile midiFile;

    // délka nejratší noty
    private float shortestNoteSec;

    private TempoMap tempoMap;
    private List<Melanchall.DryWetMidi.Interaction.Note> notes = new List<Melanchall.DryWetMidi.Interaction.Note>();


    void Start()
    {
        // naètení cesty k textu
        textPath = Path.Combine(Application.dataPath, "Resources", "Text", name + ".txt");

        // pokud ji textovı soubor neexistuje
        if (!File.Exists(textPath))
        {
            PullInfo();
            CreateTextFile();
        }
    }

    private void PullInfo()
    {
        // naètení midi souboru pomoci jména levelu
        string midiPath = Path.Combine(Application.dataPath, "Midi", name + ".mid");
        midiFile = MidiFile.Read(midiPath);

        // hodnota ètvrové noty v mikrovteøinách
        long microsecondsPerQuarterNote = (long)60000000.0 / bpm;

        // zmìna tempa midi souboru
        using (var tempoMapManager = midiFile.ManageTempoMap())
        {
            tempoMapManager.SetTempo(new MetricTimeSpan(0), new Tempo(microsecondsPerQuarterNote));
        }

        // naètení not do seznamu
        notes = midiFile.GetNotes().ToList();

        // hodnota nejkratší noty ve vteøinách
        tempoMap = midiFile.GetTempoMap();
        MetricTimeSpan firstNote = notes[0].LengthAs<MetricTimeSpan>(tempoMap);
        shortestNoteSec = (float)firstNote.TotalSeconds;
    }

    private void CreateTextFile()
    {
        try
        {
            // vytvoøení a editování textového souboru
            using (StreamWriter writer = File.CreateText(textPath))
            {
                // první øádek - index kadé noty v midi filu
                foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
                {
                    writer.Write(note.NoteNumber + " ");
                }

                writer.WriteLine();

                // druhı øádek - naètení èasu generace not do seznamu (vteøiny)
                foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
                {
                    float timestamp = (note.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1_000_000.0f);

                    // + èasová rezerva jedna ètvrtová nota
                    writer.Write(timestamp + " ");
                }

                writer.WriteLine();

                // tøetí øádek - nejkratší nota v souboru
                writer.Write(shortestNoteSec);
            }
        }

        catch (System.Exception e) 
        {
            Debug.LogError("Error writing to file: " + e.Message);
        }
    }
}