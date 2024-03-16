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
        rawImage = GetComponent<RawImage>();
        FindParent();
        VideoSwitch();
    }

    void FindParent()
    {
        parentsName = transform.parent.gameObject.name;
        currentStars = PlayerPrefs.GetInt("Level" + parentsName + "Stars");
        Debug.Log(parentsName + "ma hvezd" + currentStars);
    }

    void VideoSwitch()
    {
        //if (currentStars == 0) { vp.isLooping = true; }
        GameObject starsVideo = GameObject.Find(currentStars + "Stars");
        vp = starsVideo.GetComponent<VideoPlayer>();
        rawImage.texture = vp.targetTexture;
        vp.Play();
    }
}
