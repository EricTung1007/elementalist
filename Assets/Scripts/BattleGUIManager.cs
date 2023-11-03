using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGUIManager : MonoBehaviour
{
    // Counting the current tick number (fixed updates)
    private int fixedUpdateCount;

    // Storing the magic information
    Magic[] magics = new Magic[9 + 1]; // Using only index 1 to 9

    // Setting the parameter for element moving speed and element generation speed
    private int elementMovingInterval = 1;
    private int elementGenerationInterval = 100;

    // Storing the grid information containing elements
    GameObject[,] elementGrid = new GameObject[9, 2];

    private void Start()
    {
        SetDefaultMagics();
    }

    private void FixedUpdate()
    {
        // Generate elements if possible
        if ((fixedUpdateCount % elementGenerationInterval == 0))
        {

        }
        // Move elements if possible
        if ((fixedUpdateCount % elementMovingInterval == 0))
        {

        }

        fixedUpdateCount++;
    }

    public void LeftClickingElementTile(int xGrid, int yGrid)
    {
        Debug.Log($"Left clicked tile {xGrid} {yGrid}.");
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
    }
}
