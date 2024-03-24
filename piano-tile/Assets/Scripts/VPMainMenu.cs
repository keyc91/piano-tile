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

        // spu�t�n� druh�ho videa po konci prvn�ho
        vp.loopPointReached += Video2;
    }

    void Video1()
    {
        // na�ten� a spu�ten� prvn�ho videa
        vp.url = (Application.dataPath + "/Resources/Video/intro.mp4");
        vp.Prepare();
        vp.Play();
    }

    void Video2(VideoPlayer source)
    {
        // vypnut� loopPointReached - aby se video nena��talo v�ckr�t
        vp.loopPointReached -= Video2;

        // na�ten� druh�ho videa
        vp.url = (Application.dataPath + "/Resources/Video/intro loop.mp4");
        vp.Prepare();

        // spust� video p�ehr�vaj�c� se ve smy�ce
        vp.Play();
        vp.isLooping = true;
    }
}
