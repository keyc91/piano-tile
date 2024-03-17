using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static bool allowTouchInput;

    void Update()
    {
        // allowTouchInput - GameControl.GameStop()
        if (allowTouchInput)
        {
            // pokud hrac kliknul - spusteni Hit() z note scriptu
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

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
