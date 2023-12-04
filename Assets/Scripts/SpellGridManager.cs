using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class SpellGridManager : MonoBehaviour
{
    private const int spellAmount = 9;
    public int selectedSpellTileNumber;
    private SpellId selectedSpell;
    public GameObject[] spellTile;
    [SerializeField] private ElementGridManager elementGridManager;

    private void Awake()
    {
        spellTile = new GameObject[spellAmount + 1];
        CaptureSpellTiles();

        SpellId[] defaultSpell = new SpellId[spellAmount + 1]{
            SpellId.none,
            SpellId.fireArrow,
            SpellId.waterBall,
            SpellId.woodenArrow,
            SpellId.firePillar,
            SpellId.heal,
            SpellId.poisonBomb,
            SpellId.steamExplosion,
            SpellId.transformMud,
            SpellId.vinePull
        };
        AssignSpellTiles(spellTile, defaultSpell);
    }

    private void Start()
    {
        // Default spell selection
        SelectSpellTile(1);
    }

    private void CaptureSpellTiles()
    {
        
        for (int i = 1; i < spellAmount + 1; i++)
        {
            spellTile[i] = GameObject.Find($"SpellTile {i}");
        }
    }

    private void AssignSpellTiles(GameObject[] spellTile, SpellId[] defaultSpell)
    {
        for (int i = 1; i < spellAmount + 1; i++)
        {
            spellTile[i].GetComponent<SpellCost>().spellId = defaultSpell[i];
        }
    }
    public void SelectSpellTile(int tilenumber)
    {
        //Debug.Log($"Selecting spell tile {tilenumber}.");
        selectedSpellTileNumber = tilenumber;
        selectedSpell = spellTile[tilenumber].GetComponent<SpellCost>().spellId;

        // Highlight
        for(int i = 1; i < spellAmount + 1; i++)
        {
            spellTile[i].GetComponent<Image>().color = Color.gray;
        }
        spellTile[selectedSpellTileNumber].GetComponent<Image>().color = Color.white;
    }

    [SerializeField] private UnityEvent<SpellId> PerformSpell;

    public void TryPerformSpell(int x, int y)
    {        
        // Don't perform spell null
        if (spellTile[selectedSpellTileNumber].GetComponent<SpellCost>().spellId == SpellId.none) return;

        // ask if able to perform selected spell
        Type[,] cost = spellTile[selectedSpellTileNumber].GetComponent<SpellCost>().cost;

        bool CanPerform = elementGridManager.MatchSpellCost(cost, x, y);
        if (CanPerform)
        {
            //Debug.Log($"Performing spell {selectedSpell.ToString()}...");
            PerformSpell?.Invoke(selectedSpell);
            elementGridManager.RemoveUsedElements(cost, x, y);
        }
    }
}
