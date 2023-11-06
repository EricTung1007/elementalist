using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCost : MonoBehaviour
{
    public SpellId spellId;
    public Type[,] cost;
    [SerializeField] private Sprite fireArrowIcon, acidBombIcon, steamExplosionIcon, vinePullIcon, transformMudIcon, burningShieldIcon, healIcon, elementSurgeIcon;
    [SerializeField] private Sprite fireArrowCostSprite, acidBombCostSprite, steamExplosionCostSprite, vinePullCostSprite,
        transformMudCostSprite, burningShieldCostSprite, healCostSprite, elementSurgeCostSprite;

    private void Start()
    {
        // Set the element cost to perform this magic
        switch (spellId)
        {
            case SpellId.fireArrow:
                cost = new Type[2, 2]
                {
                    {Type.none, Type.fire},
                    {Type.fire, Type.fire}
                }; 
                break;
            case SpellId.acidBomb:
                cost = new Type[2, 2]
                {
                    {Type.none, Type.grass},
                    {Type.grass, Type.water}
                }; 
                break;
            case SpellId.steamExplosion:
                cost = new Type[2, 2]
                {
                    {Type.fire, Type.water},
                    {Type.water, Type.fire}
                }; 
                break;
            case SpellId.vinePull:
                cost = new Type[3, 2]
                {
                    {Type.none, Type.grass},
                    {Type.grass, Type.none},
                    {Type.none, Type.grass},
                }; 
                break;
            case SpellId.transformMud:
                cost = new Type[3, 1]
                {
                    {Type.water},
                    {Type.water},
                    {Type.water}
                }; 
                break;
            case SpellId.burningShield:
                cost = new Type[2, 2]
                {
                    {Type.grass, Type.grass},
                    {Type.fire, Type.fire}
                }; 
                break;
            case SpellId.heal:
                cost = new Type[4, 1]
                {
                    {Type.water},
                    {Type.grass},
                    {Type.grass},
                    {Type.water}
                }; 
                break;
            case SpellId.elementSurge:
                cost = new Type[5, 1]
                {
                    {Type.fire},
                    {Type.water},
                    {Type.grass},
                    {Type.water},
                    {Type.fire}
                }; 
                break;
        }
    }
}
