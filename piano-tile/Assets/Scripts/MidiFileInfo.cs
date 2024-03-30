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
        // Access the first line
        string firstLine = lines[0];

        // Trim leading and trailing whitespace
        firstLine = firstLine.Trim();

        // Split the first line into individual numbers using spaces
        string[] firstLineSplit = firstLine.Split(' ');

        // Convert each number to integer
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
        // Access the second line
        string secondLine = lines[1];

        // Trim leading and trailing whitespace
        secondLine = secondLine.Trim();

        // Split the second line into individual numbers using spaces
        string[] secondLineSplit = secondLine.Split(' ');

        // Convert each number to integer
        foreach (string str in secondLineSplit)
        {
            timeStamps.Add(float.Parse(str));
        }

        Debug.Log("Second line numbers: " + string.Join(", ", timeStamps));
    }

    private void ReadThirdLine()
    {
        shortestNoteSec = float.Parse(lines[2]);
        Debug.Log(shortestNoteSec);
    }

    // slovník s bpm hodnotou každého levelu
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