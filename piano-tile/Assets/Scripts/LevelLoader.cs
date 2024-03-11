using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;
    public static LevelLoader Instance;

    void Start()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }
}
