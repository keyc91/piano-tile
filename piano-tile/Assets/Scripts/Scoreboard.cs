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

        // reset sk�re
        scorepoints = 0;
    }

    public void ScoreUp()
    {
        // zv��en� sk�re
        scorepoints++;

        // zm�na sk�re na obrazovce
        text.text = scorepoints.ToString();
    }
}