using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManagerMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

            if (hit.collider != null)
            {
                hit.collider.GetComponent<MonoBehaviour>().SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
