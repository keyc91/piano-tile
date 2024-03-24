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
        // nalezen� RawImage komponentu dan�ho game objectu
        rawImage = GetComponent<RawImage>();

        // zji�t�n� po�tu hv�zd ze jm�na rodi�e
        FindParent();

        // na�ten� RawImage
        VideoSwitch();
    }

    void FindParent()
    {
        parentsName = transform.parent.gameObject.name;
        currentStars = PlayerPrefs.GetInt("Level" + parentsName + "Stars");
    }

    void VideoSwitch()
    {
        // nalezen� video p�ehr�va�e 
        GameObject starsVideo = GameObject.Find(currentStars + "Stars");
        vp = starsVideo.GetComponent<VideoPlayer>();

        // vyu�it� jeho RawImage
        rawImage.texture = vp.targetTexture;
        vp.Play();
    }
}
