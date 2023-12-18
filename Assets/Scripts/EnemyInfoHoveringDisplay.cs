using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyInfoHoveringDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public int enemyPos;
    Player player;
    public GameObject BattleController;

    //TextMeshProUGUI description;
    [SerializeField] GameObject infoBox;
    private void Awake()
    {
        //description = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();
        BattleController = GameObject.Find("BattleController");
        infoBox = GameObject.Find("Description");
    }
    public void Start()
    {
        player = BattleController.GetComponent<BattleController>().players[enemyPos];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string info = "";
        switch (player.playerId)
        {
            case "green_slime":
                info += " < 綠史萊姆 > ";
                break;
            case "blue_slime":
                info += " < 藍史萊姆 > ";
                break;
            case "red_slime":
                info += " < 紅史萊姆 > ";
                break;
        }
        info += "";

        Spell spell = player.skill[player.intention];
        switch (spell.spellId)
        {
            case SpellId.dodge:
                info += "懦弱打擊：造成 4 點傷害，並躲到隊伍最後方";
                break;
            case SpellId.grassCollide:
                info += "撞擊：造成 3 點傷害";
                break;
            case SpellId.waterCollide:
                info += "撞擊：造成 1 點傷害";
                break;
            case SpellId.fireCollide:
                info += $"熔岩噴吐：造成 {20 + player.chi} 點傷害";
                break;
            case SpellId.slime:
                info += "黏液：消耗 3 水元素，在玩家的元素槽中生成黏液\n！黏液無法主動釋放；黏液碰觸到左方時自然消除";
                break;
            case SpellId.healAllEnemy:
                info += $"全體回復：回復所有友方 {5 + player.chi} ( = 5 + 木元素) 生命；消耗所有木元素";
                break;
        }

        info += "\n";

        
        switch (player.playerId)
        {
            case "green_slime":
                //info += "自然回復：木元素被釋放時，回復 3 點生命";
                break;
            case "blue_slime":
                //info += "黏液蒐集：水元素被釋放時，增加 1 黏液";
                break;
            case "red_slime":
                //info += "熔岩聚集：火元素被釋放時，立即永久降低熔岩噴吐 2 秒準備時間";
                break;
        }

        //description.text = info;
        infoBox.GetComponent<InfoBox>().currentHovering = info;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.GetComponent<InfoBox>().currentHovering = "";
    }
}
