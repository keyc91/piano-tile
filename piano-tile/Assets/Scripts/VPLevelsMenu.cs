using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VPLevelsMenu : MonoBehaviour
{
    public VideoPlayer vp;
    private string parentsName;
    private int currentStars;
    public RawImage rawImage;

    void Awake()
    {
        // nalezení RawImage komponentu daného game objectu
        rawImage = GetComponent<RawImage>();

        // zjištìní poètu hvìzd ze jména rodièe
        FindParent();

        // naètení RawImage
        VideoSwitch();
    }

    void FindParent()
    {
        parentsName = transform.parent.gameObject.name;
        currentStars = PlayerPrefs.GetInt("Level" + parentsName + "Stars");
    }

    void VideoSwitch()
    {
        // nalezení video pøehrávaèe 
        GameObject starsVideo = GameObject.Find(currentStars + "Stars");
        vp = starsVideo.GetComponent<VideoPlayer>();

        // využití jeho RawImage
        rawImage.texture = vp.targetTexture;
        vp.Play();
    }
}
