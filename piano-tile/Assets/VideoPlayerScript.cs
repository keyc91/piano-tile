using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoPlayerScript : MonoBehaviour
{
    public VideoPlayer vp;
    void Awake()
    {
        VideoSwitch();
    }

    void VideoSwitch()
    {
        vp.url = (Application.dataPath + "/Resources/Video/" + PlayerPrefs.GetInt("CurrentStars") + " stars screen.mp4");
        vp.Prepare();
        vp.Play();
    }
}