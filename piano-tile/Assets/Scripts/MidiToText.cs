using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MidiToText : MonoBehaviour
{
    // info od u�ivatele
    public new string name;
    public long bpm;

    // cesta k textov�mu souboru
    private string textPath;

    // midifile
    private MidiFile midiFile;

    // d�lka nejrat�� noty
    private float shortestNoteSec;

    private TempoMap tempoMap;
    private List<Melanchall.DryWetMidi.Interaction.Note> notes = new List<Melanchall.DryWetMidi.Interaction.Note>();


    void Start()
    {
        // na�ten� cesty k textu
        textPath = Path.Combine(Application.dataPath, "Resources", "Text", name + ".txt");

        // pokud ji� textov� soubor neexistuje
        if (!File.Exists(textPath))
        {
            PullInfo();
            CreateTextFile();
        }
    }

    private void PullInfo()
    {
        // na�ten� midi souboru pomoci jm�na levelu
        string midiPath = Path.Combine(Application.dataPath, "Midi", name + ".mid");
        midiFile = MidiFile.Read(midiPath);

        // hodnota �tvr�ov� noty v mikrovte�in�ch
        long microsecondsPerQuarterNote = (long)60000000.0 / bpm;

        // zm�na tempa midi souboru
        using (var tempoMapManager = midiFile.ManageTempoMap())
        {
            tempoMapManager.SetTempo(new MetricTimeSpan(0), new Tempo(microsecondsPerQuarterNote));
        }

        // na�ten� not do seznamu
        notes = midiFile.GetNotes().ToList();

        // hodnota nejkrat�� noty ve vte�in�ch
        tempoMap = midiFile.GetTempoMap();
        MetricTimeSpan firstNote = notes[0].LengthAs<MetricTimeSpan>(tempoMap);
        shortestNoteSec = (float)firstNote.TotalSeconds;
    }

    private void CreateTextFile()
    {
        try
        {
            // vytvo�en� a editov�n� textov�ho souboru
            using (StreamWriter writer = File.CreateText(textPath))
            {
                // prvn� ��dek - index ka�d� noty v midi filu
                foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
                {
                    writer.Write(note.NoteNumber + " ");
                }

                writer.WriteLine();

                // druh� ��dek - na�ten� �asu generace not do seznamu (vte�iny)
                foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
                {
                    float timestamp = (note.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1_000_000.0f);

                    // + �asov� rezerva jedna �tvrtov� nota
                    writer.Write(timestamp + " ");
                }

                writer.WriteLine();

                // t�et� ��dek - nejkrat�� nota v souboru
                writer.Write(shortestNoteSec);
            }
        }

        catch (System.Exception e) 
        {
            Debug.LogError("Error writing to file: " + e.Message);
        }
    }
}