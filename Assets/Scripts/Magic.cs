using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public MagicID id;
    private Type[,] cost;

    public void Activate()
    {
        // Set the element cost to perform this magic
        switch (id)
        {
            case MagicID.fireArrow:
                cost = new Type[2, 1]
                {
                    {Type.none},
                    {Type.fire}
                }; break;
            case MagicID.acidBall:
                cost = new Type[2, 2]
                {
                    {Type.none, Type.grass},
                    {Type.grass, Type.water}
                }; break;
            case MagicID.steamExplosion:
                cost = new Type[2, 2]
                {
                    {Type.fire, Type.water},
                    {Type.water, Type.fire}
                }; break;
            case MagicID.vinePull:
                cost = new Type[3, 2]
                {
                    {Type.none, Type.grass},
                    {Type.grass, Type.none},
                    {Type.none, Type.grass},
                }; break;
            case MagicID.transformMud:
                cost = new Type[3, 1]
                {
                    {Type.water},
                    {Type.water},
                    {Type.water}
                }; break;
            case MagicID.burningShield:
                cost = new Type[2, 2]
                {
                    {Type.grass, Type.grass},
                    {Type.fire, Type.fire}
                }; break;
            case MagicID.heal:
                cost = new Type[4, 1]
                {
                    {Type.water},
                    {Type.grass},
                    {Type.grass},
                    {Type.water}
                }; break;
            case MagicID.elementSurge:
                cost = new Type[5, 1]
                {
                    {Type.fire},
                    {Type.water},
                    {Type.grass},
                    {Type.water},
                    {Type.fire}
                }; break;
        }
    }
}
