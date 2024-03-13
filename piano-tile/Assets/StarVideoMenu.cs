using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class StarVideoMenu : MonoBehaviour
{
    public VideoPlayer vp;
    public string parentsName;
    private int currentStars;

    void Awake()
    {
        FindParent();
        VideoSwitch();
    }

    void FindParent()
    {
        string parentsName = transform.parent.gameObject.name;
        currentStars = PlayerPrefs.GetInt("Level" + parentsName + "Stars");
        Debug.Log(parentsName + "ma hvezd" + currentStars);
    }

    void VideoSwitch()
    {
        //if (currentStars == 0) { vp.isLooping = true; }
        vp.url = (Application.dataPath + "/Video/menustars" + currentStars + ".mp4");
        vp.Prepare();
        vp.Play();
    }
}
