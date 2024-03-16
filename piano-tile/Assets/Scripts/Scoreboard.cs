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
        scorepoints = 0;
        Instance = this;
        SetUp();
    }

    private void SetUp()
    {
        text.rectTransform.anchoredPosition = new Vector2(-Screen.width / 8, Screen.height / 5);
        text.rectTransform.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 10);
        scorepoints = -1;
        ScoreUp();
    }

    public void ScoreUp()
    {
        scorepoints++;
        text.text = scorepoints.ToString();
    }
}