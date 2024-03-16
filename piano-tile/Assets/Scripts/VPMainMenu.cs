using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VPMainMenu : MonoBehaviour
{
    public VideoPlayer vp;

    void Start()
    {
        Intro();
        vp.loopPointReached += Loop;
    }

    void Intro()
    {
        vp.url = (Application.dataPath + "/Resources/Video/intro.mp4");
        vp.Prepare();
        vp.Play();
    }

    void Loop(VideoPlayer source)
    {
        vp.loopPointReached -= Loop;
        vp.url = (Application.dataPath + "/Resources/Video/intro loop.mp4");

        vp.Prepare();
        vp.Play();
        vp.isLooping = true;
    }
}
