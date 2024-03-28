using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGameOver : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        string parentsName = transform.parent.gameObject.name;
        int stars = PlayerPrefs.GetInt("CurrentStars");
        animator.SetInteger("Stars", stars);
    }
}
