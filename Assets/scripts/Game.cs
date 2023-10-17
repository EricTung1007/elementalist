using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int fixedUpdateCount = 0;
    public static int elementMoveInterval = 50;
    public static int elementGenerateInterval = 100;

    private void FixedUpdate()
    {
        fixedUpdateCount++;
    }
}
