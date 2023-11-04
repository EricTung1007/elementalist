using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour
{
    [SerializeField] Type type;
    public int xGrid, yGrid;

    public Sprite spriteFire, spriteWater, spriteGrass;


    // Remove this later!
    private void Update()
    {
        Activate();
    }
    public void Activate()
    {
        SetPosition();

        // Set the sprite for this element
        switch (type)
        {
            case Type.fire:  this.GetComponent<Image>().sprite = spriteFire;  break;
            case Type.water: this.GetComponent<Image>().sprite = spriteWater; break;
            case Type.grass: this.GetComponent<Image>().sprite = spriteGrass; break;
        }
    }

    // Set the anchored position
    public void SetPosition()
    {
        float x = 60.0f + xGrid * 120.0f;
        float y = 60.0f + yGrid * 120.0f;
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
    }

    // Default selection of element generation
    Type[] generationPool = new Type[3] {Type.fire, Type.water, Type.grass};

    // Constructor; Generate a new random element
    public void GenerateRandom()
    {
        var random = new System.Random();
        this.type = generationPool[random.Next(3)];
    }
}
