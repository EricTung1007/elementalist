using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        SetCurrentState(playerObject, player);
        SetCurrentState(greenSlimeObject, greenSlime);
        SetCurrentState(blueSlimeObject, blueSlime);
        SetCurrentState(redSlimeObject, redSlime);
    }

    private void SetCurrentState(GameObject entityObject, Player entity)
    {
        if (entity.GetHP() <= 0) // DEAD
        {
            entityObject.SetActive(false);
            return;
        }

        GameObject hpBar = entityObject.transform.GetChild(0).gameObject;
        GameObject statusEffect = entityObject.transform.GetChild(1).gameObject;
        GameObject intention = entityObject.transform.GetChild(2).gameObject;
        GameObject model = entityObject.transform.GetChild(3).gameObject;

        string hpBarText = $"{entity.GetHP()} / {entity.maxhp}";
        hpBar.GetComponent<TextMeshProUGUI>().text = hpBarText;

        string statusEffectText = "";
        foreach (Effect effect in entity.sustainedEffect)
        {
            statusEffectText += $"{effect.effectId} {effect.hp}({effect.duration})\n";
        }
        statusEffect.GetComponent<TextMeshProUGUI>().text = statusEffectText;

        string intentionText;
        if (entity.intention == -1)
        {
            intentionText = "";
        }
        else
        {
            intentionText = $"{entity.skill[entity.intention].spellId.ToString()}({entity.releaseIn})";
        }
        intention.GetComponent<TextMeshProUGUI>().text = intentionText;

        if(entity.playerId != "player")
        {
            float pos = entity.position;
            entityObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos * 240f - 120f, 0, 0);
        }

        /*if (entity.isHurt)
        {
            model.GetComponent<Animation>().Play("PlayerHurt");
        }*/
    }
}
