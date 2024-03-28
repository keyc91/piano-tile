using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStarCount : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        string parentsName = transform.parent.gameObject.name;
        int stars = PlayerPrefs.GetInt("Level" + parentsName + "Stars");
        animator.SetInteger("Stars", stars);
    }
}
