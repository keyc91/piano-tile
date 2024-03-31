using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static bool allowTouchInput;

    void Update()
    {
        // bool allowTouchInput - kv�li zastaven� hry skrz StopGame() (GameControl script)
        if (allowTouchInput)
        {
            // pokud hr�� kliknul
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // oblast doteku ve world coordinates
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                // spu�t�n� Hit() (Note script dot�en� noty)
                if (hit.collider != null)
                {
                    Note noteComponent = hit.collider.GetComponent<Note>();
                    if (noteComponent != null )
                    {
                        noteComponent.Hit();
                    }
                }
            }
        }
    }
}
