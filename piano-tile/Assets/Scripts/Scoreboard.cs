using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static int scorepoints;
    public static Scoreboard Instance;

    void Awake()
    {
        Instance = this;

        // reset skóre
        scorepoints = 0;
    }

    public void ScoreUp()
    {
        // zvýšení skóre
        scorepoints++;

        // zmìna skóre na obrazovce
        text.text = scorepoints.ToString();
    }
}