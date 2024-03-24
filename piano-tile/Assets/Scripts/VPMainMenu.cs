using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VPMainMenu : MonoBehaviour
{
    public VideoPlayer vp;

    void Start()
    {
        Video1();

        // spuštìní druhého videa po konci prvního
        vp.loopPointReached += Video2;
    }

    void Video1()
    {
        // naètení a spuštení prvního videa
        vp.url = (Application.dataPath + "/Resources/Video/intro.mp4");
        vp.Prepare();
        vp.Play();
    }

    void Video2(VideoPlayer source)
    {
        // vypnutí loopPointReached - aby se video nenaèítalo víckrát
        vp.loopPointReached -= Video2;

        // naètení druhého videa
        vp.url = (Application.dataPath + "/Resources/Video/intro loop.mp4");
        vp.Prepare();

        // spustí video pøehrávající se ve smyèce
        vp.Play();
        vp.isLooping = true;
    }
}
