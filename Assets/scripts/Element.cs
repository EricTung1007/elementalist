using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public GameObject controller;
    //public GameObject element;

    public int xGrid, yGrid;
    public Type type;
    public Sprite fire, water, grass;
    

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Element e = this.GetComponent<Element>();
        switch(e.type)
        {
            case Type.fire:
                this.name = "fire";
                this.GetComponent<SpriteRenderer>().sprite = fire; break;
            case Type.water:
                this.name = "water";
                this.GetComponent<SpriteRenderer>().sprite = water; break;
            case Type.grass:
                this.name = "grass";
                this.GetComponent<SpriteRenderer>().sprite = grass; break;
        }
        SetWorldCoordinate();
    }

    public void SetWorldCoordinate()
    {
        float x = xGrid, y = yGrid;
        this.transform.position = new Vector3(x - 4.0f, y - 0.7f, -1.0f);
    }

    public void OnMouseDown()
    {
        Destroy(this.gameObject);
    }
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            int x = xGrid;
            int y = yGrid;
            controller.GetComponent<Controller>().TryMagic(x, y);
        }
    }
}
