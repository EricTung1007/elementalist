using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public string id;
    public int position = -1;
    public int xSize, ySize; // the size of pattern
    public Type[,] pattern;

    public GameObject controller;

    public Sprite fireArrow, acidBall, steamExplosion, vinePull, transformMud, burningShield, heal, elementSurge;
    public string tmp;
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        this.name = id;
        Magic e = this.GetComponent<Magic>();
        switch (e.id)
        {
            case "fireArrow":
                xSize = 2; ySize = 1;
                pattern = new Type[2, 1]{
                    { Type.fire }, 
                    { Type.fire }
                };
                tmp = "fire 2";
                this.GetComponent<SpriteRenderer>().sprite = fireArrow; break;
            case "acidBall":
                xSize = 2; ySize = 2;
                pattern = new Type[2, 2]{
                    { Type.none, Type.water },
                    { Type.water, Type.grass }
                };
                tmp = "grass 6";
                this.GetComponent<SpriteRenderer>().sprite = acidBall; break;
            case "steamExplosion":
                xSize = 2; ySize = 2;
                pattern = new Type[2, 2]{
                    { Type.fire, Type.water },
                    { Type.water, Type.fire }
                };
                tmp = "none 10";
                this.GetComponent<SpriteRenderer>().sprite = steamExplosion; break;
            case "vinePull":
                xSize = 3; ySize = 2;
                pattern = new Type[3, 2]{
                    { Type.none, Type.grass },
                    { Type.grass, Type.none },
                    { Type.none, Type.grass }
                };
                this.GetComponent<SpriteRenderer>().sprite = vinePull; break;
            case "transformMud":
                xSize = 3; ySize = 1;
                pattern = new Type[3, 1]{
                    { Type.water},
                    { Type.water},
                    { Type.water}
                };
                tmp = "water 10";
                this.GetComponent<SpriteRenderer>().sprite = transformMud; break;
            case "burningShield":
                this.GetComponent<Transform>().localScale = new Vector3(0.12f, 0.12f, 0.12f);

                xSize = 2; ySize = 2;
                pattern = new Type[2, 2]{
                    { Type.grass, Type.grass },
                    { Type.fire, Type.fire }
                };
                tmp = "fire 10";
                this.GetComponent<SpriteRenderer>().sprite = burningShield; break;
            case "heal":
                xSize = 4; ySize = 1;
                pattern = new Type[4, 1]{
                    { Type.water},
                    { Type.grass},
                    { Type.grass},
                    { Type.water}
                };
                this.GetComponent<SpriteRenderer>().sprite = heal; break;
            case "elementSurge":
                xSize = 5; ySize = 1;
                pattern = new Type[5, 1]{
                    { Type.grass},
                    { Type.water},
                    { Type.grass},
                    { Type.water},
                    { Type.fire}
                };
                tmp = "forever";
                this.GetComponent<SpriteRenderer>().sprite = elementSurge; break;
        }
        SetWorldCoordinate();
    }
    public void SetWorldCoordinate()
    {
        float x = position;
        this.transform.position = new Vector3(2.2f * x - 7.6f, - 3.3f, -1.0f);
    }

    private void OnMouseDown()
    {
        controller.GetComponent<Controller>().selectedMagic = id;
    }
}
