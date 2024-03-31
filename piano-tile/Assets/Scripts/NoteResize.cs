using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteResize : MonoBehaviour
{
    public Note notePrefab;
    public static float spawnHeight;

    void Awake()
    {
        if (Time.time < 10) SetDataForNoteGeneration();
    }

    private void SetDataForNoteGeneration()
    {
        // pøizpùsobení velikosti na specifickou obrazovku
        var topRight = new Vector2(Screen.width, Screen.height);
        var topRightWorldPoint = Camera.main.ScreenToWorldPoint(topRight);
        var bottomLeftWorldPoint = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
        var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;

        // výška a délka not
        float noteHeight = screenHeight / 4;
        float noteWidth = screenWidth / 4;

        var noteSpriteRenderer = notePrefab.GetComponent<SpriteRenderer>();

        // zmìna velikosti prefabu noty
        notePrefab.transform.localScale = new Vector3(
               noteWidth / noteSpriteRenderer.bounds.size.x * noteSpriteRenderer.transform.localScale.x,
               noteHeight / noteSpriteRenderer.bounds.size.y * noteSpriteRenderer.transform.localScale.y, 1);

        spawnHeight = ((topRightWorldPoint.y * 5) / 4);

        // výpoèet mista vzniku not z velikosti obrazovky
        float leftSpawn = -noteWidth * 3 / 2;
        float leftMiddleSpawn = -noteWidth / 2;
        float rightMiddleSpawn = noteWidth / 2;
        float rightSpawn = noteWidth * 3 / 2;

        GameControl.spawns.Clear();
        GameControl.spawns.Add(leftMiddleSpawn);
        GameControl.spawns.Add(rightMiddleSpawn);
        GameControl.spawns.Add(leftSpawn);
        GameControl.spawns.Add(rightSpawn);

        GameControl.noteHeight = noteHeight;
    }
}
