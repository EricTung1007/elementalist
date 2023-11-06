using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellGridManager : MonoBehaviour
{
    private int selectedSpellTileNumber;
    private SpellId selectedSpell;
    private GameObject[] spellTile;
    private SpellCost[] spellcost;
    [SerializeField] private ElementGridManager elementGridManager;

    private void Awake()
    {
        spellTile = new GameObject[9 + 1];
        CaptureSpellTiles();
    }

    private void Start()
    {
        // Default spell selection
        SelectSpellTile(1);
    }

    private void CaptureSpellTiles()
    {
        SpellId[] defaultSpell = new SpellId[9 + 1]{
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
        for (int i = 1; i < 9 + 1; i++)
        {
            spellTile[i] = GameObject.Find($"SpellTile {i}");
            spellTile[i].GetComponent<SpellCost>().spellId = defaultSpell[i];
        }
    }
    public void SelectSpellTile(int tilenumber)
    {
        Debug.Log($"Selecting spell tile {tilenumber}.");
        selectedSpellTileNumber = tilenumber;
        selectedSpell = spellTile[tilenumber].GetComponent<SpellCost>().spellId;
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
            PerformSpell?.Invoke(selectedSpell);
            elementGridManager.RemoveUsedElements(cost, x, y);
        }
    }
}
