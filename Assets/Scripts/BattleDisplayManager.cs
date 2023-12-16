using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BattleDisplayManager : MonoBehaviour
{
    GameObject playerObject;
    List<GameObject> enemyObject = new List<GameObject>();

    [SerializeField] GameObject enemyHolder;
    [SerializeField] GameObject greenSlimeObject, blueSlimeObject, redSlimeObject; // Prefabs
    private void Start()
    {
        playerObject = GameObject.Find("Player");
    }

    public void newWaveLoading()
    {
        enemyObject.Clear();

        List<Player> players = GetComponent<BattleController>().players;
        for (int i = 1; i < players.Count; i++)
        {
            Player enemy = players[i];

            GameObject newEnemyObject = null;
            switch(enemy.type)
            {
                case Type.grass: newEnemyObject = Instantiate(greenSlimeObject, new Vector3(0, 0, 0), Quaternion.identity, enemyHolder.transform); break;
                case Type.water: newEnemyObject = Instantiate(blueSlimeObject, new Vector3(0, 0, 0), Quaternion.identity, enemyHolder.transform); break;
                case Type.fire: newEnemyObject = Instantiate(redSlimeObject, new Vector3(0, 0, 0), Quaternion.identity, enemyHolder.transform); break;
            }
            enemyObject.Add(newEnemyObject);
        }

        Debug.Log("New Wave Loaded!");
    }

    private void Update()
    {
        // show current state
        List<Player> players = GetComponent<BattleController>().players;

        Player player  = players[0];
        SetCurrentState(playerObject, player);

        for (int i = 1; i < players.Count; i++)
        {
            Player enemy = players[i];
            SetCurrentState(enemyObject[i - 1], enemy);
        }
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
            //statusEffectText += $"{effect.effectId} {effect.hp}({effect.duration})\n";

            switch(effect.effectId)
            {
                case EffectId.poison:
                    statusEffectText += $"中毒";
                    break;
                case EffectId.dizziness:
                    statusEffectText += $"暈眩";
                    break;
                case EffectId.mud:
                    statusEffectText += $"泥濘";
                    break;
            }
            statusEffectText += $" ({effect.duration})\n";
        }
        statusEffect.GetComponent<TextMeshProUGUI>().text = statusEffectText;

        string intentionText;
        if (entity.intention == -1)
        {
            intentionText = "";
        }
        else
        {
            //intentionText = $"{entity.skill[entity.intention].spellId.ToString()}({entity.skill[entity.intention].cooldown - entity.preparedFor})";
            intentionText = "";
            Spell spell = entity.skill[entity.intention];
            switch(spell.spellId)
            {
                case SpellId.dodge:
                    intentionText += "懦弱打擊";
                    break;
                case SpellId.grassCollide:
                case SpellId.waterCollide:
                    intentionText += "撞擊";
                    break;
                case SpellId.fireCollide:
                    intentionText += "熔岩噴吐";
                    break;
                case SpellId.slime:
                    intentionText += "黏液";
                    break;
                case SpellId.healAllEnemy:
                    intentionText += "全體回復";
                    break;
            }
            intentionText += $" ({System.Math.Max(1, entity.skill[entity.intention].cooldown - entity.preparedFor)})\n";
        }
        intention.GetComponent<TextMeshProUGUI>().text = intentionText;

        if(entity.playerId != "player")
        {
            float pos = entity.position;
            entityObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos * 240f - 120f, 0, 0);
        }

        if (entity.isHurt)
        {
            model.GetComponent<Animation>().Play();
        }
    }
}
