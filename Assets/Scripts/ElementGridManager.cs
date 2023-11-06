using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementGridManager : MonoBehaviour
{
    // Counting the current tick number (fixed updates)
    private int fixedUpdateCount = 0;
    // Setting the parameter for element moving speed and element generation speed
    private int elementMovingInterval = 1;
    private int elementGenerationInterval = 100;

    public GameObject elementHolder;
    public GameObject elementPrefab;

    private int gridWidth = 9;
    private int gridHeight = 2;
    // Storing the grid information containing elements
    GameObject[,] elementGrid = new GameObject[9, 2];

    private void Start()
    {
        GenerateInitialElements();
    }

    private void FixedUpdate()
    {
        if (fixedUpdateCount % elementMovingInterval == 0)
        {
            MoveElements();
        }
        // Generate elements if possible
        if (fixedUpdateCount % elementGenerationInterval == 0)
        {
            GenerateElements();
        }

        fixedUpdateCount++;
    }

    private void GenerateInitialElements()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++) 
            {
                elementGrid[i, j] = GenerateElement(i, j);
            }
        }
    }
    private void MoveElements()
    {
        for (int i = 1; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if ((elementGrid[i - 1, j] == null) && (elementGrid[i, j] != null))
                {
                    elementGrid[i - 1, j] = elementGrid[i, j];
                    elementGrid[i - 1, j].GetComponent<Element>().xGrid = i - 1;
                    elementGrid[i - 1, j].GetComponent<Element>().SetPosition();

                    elementGrid[i, j] = null;
                }
            }
        }
    }
    private void GenerateElements()
    {
        int i = gridWidth - 1;
        for (int j = 0; j < gridHeight; j++)
        {
            if (elementGrid[i, j] == null)
            {
                elementGrid[i, j] = GenerateElement(i, j);
            }
        }
    }
    private GameObject GenerateElement(int xGrid, int yGrid)
    {
        GameObject newElement = Instantiate(elementPrefab, new Vector3(0, 0, 0), Quaternion.identity, elementHolder.transform);
        Element element = newElement.GetComponent<Element>();

        // Assign a random element type to it
        element.Generate();
        element.xGrid = xGrid; element.yGrid = yGrid;

        element.Activate();
        elementGrid[xGrid, yGrid] = newElement;

        return newElement;
    }
    public void ReleaseElement(int xGrid, int yGrid)
    {
        RemoveElement(elementGrid[xGrid, yGrid]);
    }
    private void RemoveElement(GameObject element)
    {
        Destroy(element);
    }
    public bool MatchSpellCost(Type[,] cost, int x, int y)
    {
        bool matechedSpellCost = true;
        int xCost = cost.GetUpperBound(0) + 1;
        int yCost = cost.GetUpperBound(1) + 1;

        if ((x + xCost > gridWidth) || (y + yCost > gridHeight))
        {
            matechedSpellCost = false;
            return matechedSpellCost;
        }
        for (int i = 0; i < xCost; i++)
        {
            for (int j = 0; j < yCost; j++)
            {
                if (elementGrid[x + i, y + j].GetComponent<Element>().type != cost[i, j])
                {
                    matechedSpellCost = false;
                    return matechedSpellCost;
                }
            }
        }

        return matechedSpellCost;
    }
}
