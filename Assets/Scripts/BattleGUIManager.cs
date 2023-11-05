using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleGUIManager : MonoBehaviour
{
    // Counting the current tick number (fixed updates)
    private int fixedUpdateCount = 0;

    // Storing the magic information
    GameObject[] magicGrid = new GameObject[9 + 1]; // Using only index 1 to 9

    [SerializeField] SpellId selectedMagic;

    // Setting the parameter for element moving speed and element generation speed
    private int elementMovingInterval = 1;
    private int elementGenerationInterval = 100;

    private int gridWidth = 9;
    private int gridHeight = 2;
    // Storing the grid information containing elements
    GameObject[,] elementGrid = new GameObject[9, 2];


    GameObject selectedMagicTile;
    GameObject[,] elementTile = new GameObject[9, 2];
    [SerializeField] Sprite highlight;

    public GameObject elementPrefab;
    public GameObject elementHolder;

    [SerializeField] private UnityEvent<SpellId> _performMagic;

    private void Start()
    {
        //CaptureMagicTile();
        //CaptureElementTile();

        //SetDefaultMagics();
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
        TryRemovingElement(xGrid, yGrid);
    }
    public void RightClickingElementTile(int xGrid, int yGrid)
    {
        Debug.Log($"Right clicked tile {xGrid} {yGrid}.");
    }

    private int xGridHovering, yGridHovering;
    public void EnteringElementTile(int xGrid, int yGrid)
    {
        xGridHovering = xGrid; yGridHovering = yGrid;
    }
    public void ExitingElementTile(int xGrid, int yGrid)
    {
        xGridHovering = -1; yGridHovering = -1;
    }
    public void SelectingSkillTile(int number)
    {
        selectedMagicTile = magicGrid[number];
        selectedMagic = magicGrid[number].GetComponent<Magic>().id;
    }

    private void SetDefaultMagics()
    {
        SpellId[] magics = new SpellId[9 + 1]{
            SpellId.none,
            SpellId.fireArrow,
            SpellId.acidBomb,
            SpellId.steamExplosion,
            SpellId.vinePull,
            SpellId.transformMud,
            SpellId.burningShield,
            SpellId.heal,
            SpellId.elementSurge,
            SpellId.none
        };

        for(int i = 1; i < 9 + 1; i++)
        {
            magicGrid[i].GetComponent<Magic>().id = magics[i];
        }

        selectedMagicTile = magicGrid[5];
    }

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
    private void CaptureElementTile()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                elementTile[i, j] = GameObject.Find($"Tile {i} {j}");
                DeHighlightElementTile(elementTile[i, j]);
            }
        }
    }
    private void CaptureMagicTile()
    {
        for (int i = 1; i < 9 + 1; i++)
        {
            magicGrid[i] = GameObject.Find($"Magic{i}");
        }
    }
    private void DisplayMagicCost(int x, int y)
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                DeHighlightElementTile(elementTile[i, j]);
            }
        }
        if ((xGridHovering == -1) || (yGridHovering == -1)) return;

        // Try matching magic cost at the current tile selection
        // if cost cannot fit into the grid: skip
        HighlightElementTile(elementTile[xGridHovering, yGridHovering]);
    }

    private void HighlightElementTile(GameObject tile)
    {
        Image image = tile.GetComponent<Image>();
        image.sprite = highlight;
        image.color = Color.yellow;
    }
    private void DeHighlightElementTile(GameObject tile)
    {
        Image image = tile.GetComponent<Image>();
        image.sprite = highlight;
        image.color = Color.clear;
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
