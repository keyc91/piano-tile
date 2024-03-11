using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerMainMenu : MonoBehaviour
{
    public VideoPlayer vp;

    void Start()
    {
        Intro();
        vp.loopPointReached += Loop;
    }

    void Intro()
    {
        vp.url = (Application.dataPath + "/video/intro.mp4");
        vp.Prepare();
        vp.Play();
    }

    void Loop(VideoPlayer source)
    {
        Debug.Log("loop reached");
        vp.loopPointReached -= Loop;
        vp.url = (Application.dataPath + "/video/intro loop.mp4");
        vp.isLooping = true;
        vp.Prepare();
        vp.Play();
    }
}
