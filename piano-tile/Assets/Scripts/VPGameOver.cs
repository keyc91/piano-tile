using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VPGameOver : MonoBehaviour
{
    public VideoPlayer vp;

    void Awake()
    {
        VideoSwitch();
    }

    void VideoSwitch()
    {
        // nalezen� a spu�t�n� videa z po�tu dosa�en�ch hv�zd
        vp.url = (Application.dataPath + "/Resources/Video/gameover" + PlayerPrefs.GetInt("CurrentStars") + ".mp4");
        vp.Prepare();
        vp.Play();
    }
}