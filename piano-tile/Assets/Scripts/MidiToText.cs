using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MidiToText : MonoBehaviour
{
    public new string name;

    private string textPath;
    private MidiFile midiFile;
    private float shortestNoteSec;

    private TempoMap tempoMap;
    private List<Melanchall.DryWetMidi.Interaction.Note> notes = new List<Melanchall.DryWetMidi.Interaction.Note>();


    void Start()
    {
        textPath = Path.Combine(Application.dataPath, "Resources", "Text", name + ".txt");

        if (!File.Exists(textPath))
        {
            PullInfo();
            CreateTextFile();
        }
    }

    private void PullInfo()
    {
        // naËtenÌ midi souboru pomoci jmÈna levelu
        string midiPath = Path.Combine(Application.dataPath, "Midi", name + ".mid");
        midiFile = MidiFile.Read(midiPath);

        // p¯epis bpm pokud je definov·no, jinak v˝chozÌ hodnota 120
        long bpm = bpms.ContainsKey(name) ? bpms[name] : 120;

        // hodnota ËtvrùovÈ noty v mikrovte¯in·ch
        long microsecondsPerQuarterNote = (long)60000000.0 / bpm;

        // zmÏna tempa midi souboru
        using (var tempoMapManager = midiFile.ManageTempoMap())
        {
            tempoMapManager.SetTempo(new MetricTimeSpan(0), new Tempo(microsecondsPerQuarterNote));
        }

        // naËtenÌ not do seznamu
        notes = midiFile.GetNotes().ToList();

        // hodnota nejkratöÌ noty ve vte¯in·ch
        tempoMap = midiFile.GetTempoMap();
        MetricTimeSpan firstNote = notes[0].LengthAs<MetricTimeSpan>(tempoMap);
        shortestNoteSec = (float)firstNote.TotalSeconds;
    }

    private void CreateTextFile()
    {
        try
        {
            // vytvo¯enÌ a editov·nÌ textovÈho souboru
            using (StreamWriter writer = File.CreateText(textPath))
            {
                // prvnÌ ¯·dek - index kaûdÈ noty v midi filu
                foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
                {
                    writer.Write(note.NoteNumber + " ");
                }

                writer.WriteLine();

                // druh˝ ¯·dek - naËtenÌ Ëasu generace not do seznamu (vte¯iny)
                foreach (Melanchall.DryWetMidi.Interaction.Note note in notes)
                {
                    float timestamp = (note.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1_000_000.0f);

                    // + Ëasov· rezerva jedna Ëtvrtov· nota
                    writer.Write(timestamp + " ");
                }

                writer.WriteLine();

                // t¯etÌ ¯·dek - nejkratöÌ nota v souboru
                writer.Write(shortestNoteSec);
            }
        }

        catch (System.Exception e) 
        {
            Debug.LogError("Error writing to file: " + e.Message);
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
}