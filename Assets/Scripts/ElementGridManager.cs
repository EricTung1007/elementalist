using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ElementGridManager : MonoBehaviour
{
    // Counting the current tick number (fixed updates)
    private int fixedUpdateCount = 0;
    // Setting the parameter for element moving speed and element generation speed
    private int elementMovingInterval = 5;
    private int elementGenerationInterval = 100;

    public GameObject elementHolder;
    public GameObject elementPrefab;

    private int xGridHovering = -1, yGridHovering = -1;

    private int gridWidth = 9;
    private int gridHeight = 2;
    // Storing the grid information containing elements
    GameObject[,] elementGrid = new GameObject[9, 2];
    GameObject[,] elementTiles = new GameObject[9, 2];
    public int debuffGoo;

    [SerializeField] private UnityEvent<Type> ReleasedElementEffect;

    private void Start()
    {
        GenerateInitialElements();
        CaptureElementTiles();
        debuffGoo = 0;
    }

    private void FixedUpdate()
    {
        if (fixedUpdateCount % elementMovingInterval == 0)
        {
            MoveElements();
            //RemoveGooOnSide();
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
            for (int j = 0; j < gridHeight; j++)
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
        var random = new System.Random();
        Type[] defaultGenerationPool = new Type[3] { Type.fire, Type.water, Type.grass };

        element.type = Type.none;
        if ((debuffGoo > 0) && (random.Next(100) < 50))
        {
            element.type = Type.goo;
            debuffGoo--;
        }

        // If no debuff applied, generate default elements
        if (element.type == Type.none)
        {
            element.type = defaultGenerationPool[random.Next(3)];
        }


        element.xGrid = xGrid; element.yGrid = yGrid;

        element.Activate();
        elementGrid[xGrid, yGrid] = newElement;

        return newElement;
    }

    public void ApplyDebuffGoo()
    {
        debuffGoo++;
    }
    public void ReleaseElement(int xGrid, int yGrid)
    {
        GameObject currentElement = elementGrid[xGrid, yGrid];
        if (currentElement == null) { return; }

        if (currentElement.GetComponent<Element>().type == Type.goo) { return; } // Can't release goo element
        ReleasedElementEffect?.Invoke(currentElement.GetComponent<Element>().type);
        RemoveElement(currentElement);
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
                if (cost[i, j] == Type.none) continue;
                else
                {
                    if (elementGrid[x + i, y + j] == null)
                    {
                        matechedSpellCost = false;
                        return matechedSpellCost;
                    }
                    if (elementGrid[x + i, y + j].GetComponent<Element>().type != cost[i, j])
                    {
                        matechedSpellCost = false;
                        return matechedSpellCost;
                    }
                }

            }
        }

        return matechedSpellCost;
    }
    public void RemoveUsedElements(Type[,] cost, int x, int y)
    {
        int xCost = cost.GetUpperBound(0) + 1;
        int yCost = cost.GetUpperBound(1) + 1;

        for (int i = 0; i < xCost; i++)
        {
            for (int j = 0; j < yCost; j++)
            {
                if (cost[i, j] != Type.none)
                {
                    RemoveElement(elementGrid[x + i, y + j]);
                }
            }
        }
    }

    private void RemoveGooOnSide()
    {
        int i = 0;
        for (int j = 0; j < gridHeight; j++)
        {
            GameObject element = elementGrid[i, j];
            if (element == null) continue;
            if (element.GetComponent<Element>().type == Type.goo)
            {
                Destroy(element);
            }
        }
    }

    public void RemoveAllElements()
    {
        foreach (var element in elementGrid)
        {
            if (element != null)
            {
                RemoveElement(element);
            }
        }
    }

    public void PointerEnterElementTile(int xGrid, int yGrid)
    {
        xGridHovering = xGrid;
        yGridHovering = yGrid;

        if (elementGrid[xGridHovering, yGridHovering] != null)
        {
            elementGrid[xGridHovering, yGridHovering].GetComponent<Animation>().Rewind();
            elementGrid[xGridHovering, yGridHovering].GetComponent<Animation>().Play();
        }

    }

    public void PointerExitElementTile(int xGrid, int yGrid)
    {
        xGridHovering = -1;
        yGridHovering = -1;
    }

    private void CaptureElementTiles()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                elementTiles[i, j] = GameObject.Find($"Tile {i} {j}");
                //Debug.Log($"{elementTiles[i, j].name}");
            }
        }
    }

    private void Update()
    {
        SpellCostHighLight();

        for (int i = 0; i < gridWidth; i++)
            for (int j = 0; j < gridHeight; j++)
            {
                if (i != xGridHovering || j != yGridHovering)
                    elementGrid[i, j].transform.localScale = new Vector3(1.0F, 1.0F);
            }
    }
    private void SpellCostHighLight()
    {

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Image image = elementTiles[i, j].GetComponent<Image>();
                image.color = Color.clear;
            }
        }
        if ((xGridHovering == -1) || (yGridHovering == -1)) { return; }

        Type[,] cost = GetComponent<SpellGridManager>().spellTile[GetComponent<SpellGridManager>().selectedSpellTileNumber].GetComponent<SpellCost>().cost;

        bool matchedSpellCost = MatchSpellCost(cost, xGridHovering, yGridHovering);

        int xCost = cost.GetUpperBound(0) + 1;
        int yCost = cost.GetUpperBound(1) + 1;
        if (matchedSpellCost)
        {
            for (int i = 0; i < xCost; i++)
            {
                for (int j = 0; j < yCost; j++)
                {
                    if (cost[i, j] == Type.none) continue;
                    Image image = elementTiles[i + xGridHovering, j + yGridHovering].GetComponent<Image>();
                    image.color = Color.yellow;
                    //image.color -= new Color(0f, 0f, 0f, 0.5f);
                }
            }
        }
    }
}
