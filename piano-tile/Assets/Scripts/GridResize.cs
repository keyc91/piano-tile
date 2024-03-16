using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridResize : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        ResizeCells();
    }

    void ResizeCells()
    {
        gridLayoutGroup.cellSize = new Vector2((float)Screen.width, gridLayoutGroup.cellSize.y);
    }
}
