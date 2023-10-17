using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public GameObject controller;
    // position
    private int xGrid = -1;
    private int yGrid = -1;

    private int counter = 0;

    // referneces for all the sprites that the element can be
    public Sprite fire, water, grass;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        // take the instantiated location and adjust the transform
        SetCoordinate();

        switch (this.name)
        {
            case "fire": this.GetComponent<SpriteRenderer>().sprite = fire; break;
            case "water": this.GetComponent<SpriteRenderer>().sprite = water; break;
            case "grass": this.GetComponent<SpriteRenderer>().sprite = grass; break;
        }
    }

    public void SetCoordinate()
    {
        float x = xGrid;
        float y = yGrid;

        x += -4.0f;
        y += -0.8f;

        this.transform.position = new Vector3 (x, y, -1.0f);
    }

    public int GetXGrid()
    {
        return xGrid;
    }

    public int GetYGrid()
    {
        return yGrid;
    }

    public void SetXGrid(int x)
    {
        xGrid = x;
    }
    public void SetYGrid(int y)
    {
        yGrid = y;
    }
    public void OnMouseDown()
    {
        Destroy(this.gameObject);
    }
}
