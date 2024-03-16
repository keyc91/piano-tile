using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class NoteRename : MonoBehaviour
{
    void Start()
    {
        AccessFiles();
    }

    void AccessFiles()
    {
        string directoryPath = Application.dataPath + "/Resources/Piano";
        if (Directory.Exists(directoryPath))
        {
            // Get all files in the directory
            string[] files = Directory.GetFiles(directoryPath);
            Debug.Log("Number of files found: " + files.Length);
            // Iterate through each file
            foreach (string filePath in files)
            {
                // Extract the file name without the extension
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                // Convert note name to MIDI note number
                int midiNote = ConvertNoteToMidi(fileName);

                // Construct the new file name (modify as needed)
                string newFileName = midiNote.ToString();

                // Get the file extension
                string fileExtension = Path.GetExtension(filePath);

               string newFilePath = Path.Combine(directoryPath, newFileName + fileExtension);

            // Rename the file
            File.Move(filePath, newFilePath);
            Debug.Log("Renamed file: " + fileName + " to " + newFileName);
        }
        }
        else
        {
            Debug.LogError("Directory does not exist: " + directoryPath);
        }
    }

    public static int ConvertNoteToMidi(string noteName)
    {
        // Split the note name into pitch and octave parts
        string pitch = noteName.Substring(0, noteName.Length - 1);

        int octave;
        bool parseSuccess = int.TryParse(noteName.Substring(noteName.Length - 1), out octave);

        if (!parseSuccess)
        {
            // Handle the case where parsing failed (e.g., invalid octave)
            Debug.LogError("Invalid note name: " + noteName);
            return -1; // Return a default value or throw an exception
        }


        // Map pitch names to MIDI note numbers
        switch (pitch)
        {
            case "c": return 12 + octave * 12;
            case "c-": return 13 + octave * 12;
            case "d": return 14 + octave * 12;
            case "d-": return 15 + octave * 12;
            case "e": return 16 + octave * 12;
            case "f": return 17 + octave * 12;
            case "f-": return 18 + octave * 12;
            case "g": return 19 + octave * 12;
            case "g-": return 20 + octave * 12;
            case "a": return 21 + octave * 12;
            case "a-": return 22 + octave * 12;
            case "b": return 23 + octave * 12;
            default: return -1; // Handle invalid note names
        }
    }
}
