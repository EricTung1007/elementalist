using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDisplayManager : MonoBehaviour
{
    GameObject playerObject, greenSlimeObject, blueSlimeObject, redSlimeObject;
    private void Start()
    {
        // Capture entity game objects

        playerObject = GameObject.Find("Player");
        greenSlimeObject = GameObject.Find("GreenSlime");
        blueSlimeObject = GameObject.Find("BlueSlime");
        redSlimeObject = GameObject.Find("RedSlime");
    }

    private void Update()
    {
        // show current state
        List<Player> players = GetComponent<BattleController>().players;

        Player player = players[0];
        Player greenSlime = players[1];
        Player blueSlime = players[2];
        Player redSlime = players[3];
    }
}
