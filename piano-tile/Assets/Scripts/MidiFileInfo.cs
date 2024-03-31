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

        // restart sc�ny
        noteNumber.Clear();
        timeStamps.Clear();

        LoadMidiFile();
    }

    private void LoadMidiFile()
    {
        // na�ten� textov�ho souboru (generovan�ho z midi) pomoci jm�na levelu
        string scene = PlayerPrefs.GetString("CurrentLevel");
        string filePath = "Text/" + scene;
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);

        Debug.Log(scene);
        if (textAsset != null)
        {
            // na�ten� textu do stringu
            string fileContents = textAsset.text;

            // rozd�len� do ��dk�
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

        // v�po�et rychlosti not
        speed = GameControl.noteHeight / shortestNoteSec;
    }

    private void ReadFirstLine()
    {
        // na�ten� prvn�ho ��dku
        string firstLine = lines[0];

        // od��znut� pr�zdn�ch hodnot
        firstLine = firstLine.Trim();

        // rozd�len� na��sla podle mezer
        string[] firstLineSplit = firstLine.Split(' ');

        // convertov�n� string� do int�
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
        // na�ten� druh�ho �adku
        string secondLine = lines[1];

        // od��znut� pr�zdn�ch hodnot
        secondLine = secondLine.Trim();

        // rozd�len� po mezer�ch
        string[] secondLineSplit = secondLine.Split(' ');

        // zm�na na int
        foreach (string str in secondLineSplit)
        {
            timeStamps.Add(float.Parse(str));
        }

        Debug.Log("Second line numbers: " + string.Join(", ", timeStamps));
    }

    private void ReadThirdLine()
    {
        // na�ten� nejkrat�� noty
        shortestNoteSec = float.Parse(lines[2]);
        Debug.Log(shortestNoteSec);
    }
}