using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour
{
    [SerializeField] public Type type;
    public int xGrid, yGrid;

    public Sprite spriteFire, spriteWater, spriteGrass, spriteGoo;


    public void Activate()
    {
        SetPosition();

        // Set the sprite for this element
        switch (type)
        {
            case Type.fire:  this.GetComponent<Image>().sprite = spriteFire;  break;
            case Type.water: this.GetComponent<Image>().sprite = spriteWater; break;
            case Type.grass: this.GetComponent<Image>().sprite = spriteGrass; break;
            case Type.goo: this.GetComponent<Image>().sprite = spriteGoo; break;
        }
    }

    // Set the anchored position
    public void SetPosition()
    {
        float x = 60.0f + xGrid * 120.0f;
        float y = 60.0f + yGrid * 120.0f;
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
    }
}
