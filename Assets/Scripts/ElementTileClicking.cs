using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementTileClicking : MonoBehaviour, IPointerDownHandler
{
    public int xGrid, yGrid;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Clicked tile {xGrid} {yGrid}.");
        // ClickingTile(xGrid, yGrid);
    }

    // private void ClickingTile(int xGrid, int yGrid);
}
