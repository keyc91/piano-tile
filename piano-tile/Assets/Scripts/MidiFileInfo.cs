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
    public static float speed;

    private string[] lines;

    public static List<float> timeStamps = new List<float>();
    public static List<int> noteNumber = new List<int>();

    public static float shortestNoteSec;

    private void Awake()
    {
        Instance = this;

        // restart scény
        noteNumber.Clear();
        timeStamps.Clear();

        LoadMidiFile();
    }

    private void LoadMidiFile()
    {
        // naètení textového souboru (generovaného z midi) pomoci jména levelu
        string scene = PlayerPrefs.GetString("CurrentLevel");
        string filePath = "Text/" + scene;
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);

        Debug.Log(scene);
        if (textAsset != null)
        {
            // naètení textu do stringu
            string fileContents = textAsset.text;

            // rozdìlení do øádkù
            lines = fileContents.Split('\n');

            ReadFirstLine();
            ReadSecondLine();
            ReadThirdLine();

            speed = GameControl.noteHeight / shortestNoteSec;
        }

        else
        {
            Debug.LogError("Text file not found.");
        }

        // výpoèet rychlosti not
        speed = GameControl.noteHeight / shortestNoteSec;
    }

    private void ReadFirstLine()
    {
        // naètení prvního øádku
        string firstLine = lines[0];

        // odøíznutí prázdných hodnot
        firstLine = firstLine.Trim();

        // rozdìlení naèísla podle mezer
        string[] firstLineSplit = firstLine.Split(' ');

        // convertování stringù do intù
        foreach (string str in firstLineSplit)
        {
            try
            {
                int parsedInt = int.Parse(str);
                noteNumber.Add(parsedInt);
            }
            catch (FormatException ex)
            {
                Debug.LogError("Error parsing string to integer: " + ex.Message);
                Debug.LogError("Invalid string: " + str);
            }
        }

        Debug.Log("First line numbers: " + string.Join(", ", noteNumber));
    }

    private void ReadSecondLine()
    {
        // naètení druhého øadku
        string secondLine = lines[1];

        // odøíznutí prázdných hodnot
        secondLine = secondLine.Trim();

        // rozdìlení po mezerách
        string[] secondLineSplit = secondLine.Split(' ');

        // zmìna na int
        foreach (string str in secondLineSplit)
        {
            timeStamps.Add(float.Parse(str));
        }

        Debug.Log("Second line numbers: " + string.Join(", ", timeStamps));
    }

    private void ReadThirdLine()
    {
        // naètení nejkratší noty
        shortestNoteSec = float.Parse(lines[2]);
        Debug.Log(shortestNoteSec);
    }
}