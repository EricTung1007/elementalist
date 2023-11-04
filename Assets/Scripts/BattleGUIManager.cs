using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGUIManager : MonoBehaviour
{
    // Counting the current tick number (fixed updates)
    private int fixedUpdateCount = 0;

    // Storing the magic information
    Magic[] magics = new Magic[9 + 1]; // Using only index 1 to 9

    [SerializeField] Magic selectedMagic;

    // Setting the parameter for element moving speed and element generation speed
    private int elementMovingInterval = 1;
    private int elementGenerationInterval = 100;

    private int gridWidth = 9;
    private int gridHeight = 2;
    // Storing the grid information containing elements
    GameObject[,] elementGrid = new GameObject[9, 2];

    private void Start()
    {
        SetDefaultMagics();
        GenerateStartingElement();
    }

    private void FixedUpdate()
    {
        // Generate elements if possible
        if ((fixedUpdateCount % elementGenerationInterval == 0))
        {
            for (int j = 0; j < gridHeight; j++)
            {
                TryGeneratingElement(gridWidth - 1, j);
            }
        }
        // Move elements if possible
        if ((fixedUpdateCount % elementMovingInterval == 0))
        {
            for (int i = 1; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    TryMovingElement(i, j);
                }
            }
        }

        fixedUpdateCount++;
    }

    public void LeftClickingElementTile(int xGrid, int yGrid)
    {
        Debug.Log($"Left clicked tile {xGrid} {yGrid}.");
        TryRemovingElement(xGrid, yGrid);
    }
    public void RightClickingElementTile(int xGrid, int yGrid)
    {
        Debug.Log($"Right clicked tile {xGrid} {yGrid}.");
    }
    public void EnteringElementTile(int xGrid, int yGrid)
    {
        Debug.Log($"Entered tile {xGrid} {yGrid}.");
    }
    public void SelectingSkillTile(int number)
    {
        selectedMagic = magics[number];
        Debug.Log($"Left clicked skill tile {magics[number]}.");
    }
    private void SetDefaultMagics()
    {
        magics = new Magic[9 + 1]
        {
            Magic.none,
            Magic.fireArrow,
            Magic.acidBall,
            Magic.steamExplosion,
            Magic.vinePull,
            Magic.transformMud,
            Magic.burningShield,
            Magic.heal,
            Magic.elementSurge,
            Magic.none
        };

        selectedMagic = magics[5];
    }

    public GameObject elementPrefab;
    public GameObject elementHolder;
    private void GenerateStartingElement()
    {
        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                GameObject newElement = GenerateElement(i, j);
            }
        }
    }

    private void TryMovingElement(int x, int y)
    {
        if ((elementGrid[x - 1, y] == null) && (elementGrid[x, y] != null))
        {
            elementGrid[x - 1, y] = elementGrid[x, y];
            elementGrid[x - 1, y].GetComponent<Element>().xGrid = x - 1;
            elementGrid[x - 1, y].GetComponent<Element>().SetPosition();

            elementGrid[x, y] = null;
        }
    }

    private void TryGeneratingElement(int x, int y)
    {
        if (elementGrid[x, y] == null)
        {
            GameObject newElement = GenerateElement(x, y);
        }
    }

    private void TryRemovingElement(int x, int y)
    {
        Destroy(elementGrid[x, y]);
    }

    private void TryShowingMagicCost()
    {
    }

    private GameObject GenerateElement(int x, int y)
    {
        // Instantiate a new element
        GameObject newElement = Instantiate(elementPrefab, new Vector3(0, 0, 0), Quaternion.identity, elementHolder.transform);
        Element element = newElement.GetComponent<Element>();

        // Assign a random element type to it
        element.GenerateRandom();
        element.xGrid = x; element.yGrid = y;

        element.Activate();
        elementGrid[x, y] = newElement;

        return newElement;
    }
}
