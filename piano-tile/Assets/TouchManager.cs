using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public bool allowTouchInput;
    public static TouchManager Instance;

    void Awake()
    {
        Instance = this;
        allowTouchInput = true;
    }

    void Update()
    {
        if (allowTouchInput)
        {
            Debug.Log("touch allowed");
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null)
                {
                    Note noteComponent = hit.collider.GetComponent<Note>();
                    if (noteComponent != null)
                    {
                        noteComponent.Hit();
                    }
                }
            }
        }
    }
}
