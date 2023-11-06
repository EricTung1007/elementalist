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

    private void Start()
    {
        // Set spell tile
    }

    private void SelectSpellTile(int tilenumber)
    {
        selectedSpellTileNumber = tilenumber;
        selectedSpell = spellTile[tilenumber].GetComponent<SpellCost>().spellId;
    }

    [SerializeField] private UnityEvent<SpellId> PerformSpell;

    public void TryPerformSpell(int x, int y)
    {
        // ask if able to perform selected spell
        Type[,] cost = spellTile[selectedSpellTileNumber].GetComponent<SpellCost>().cost;
        bool CanPerform = elementGridManager.MatchSpellCost(cost, x, y);

        if (CanPerform)
        {
            PerformSpell?.Invoke(selectedSpell);
        }
    }
}
