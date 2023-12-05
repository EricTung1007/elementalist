using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyInfoHoveringDisplay : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private int enemyPos;
    Player player;
    public GameObject BattleController;

    TextMeshProUGUI description;
    private void Awake()
    {
        description = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();
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
                info += " < ��v�ܩi > ";
                break;
            case "blue_slime":
                info += " < �ťv�ܩi > ";
                break;
            case "red_slime":
                info += " < ���v�ܩi > ";
                break;
        }
        info += "";

        Spell spell = player.skill[player.intention];
        switch (spell.spellId)
        {
            case SpellId.dodge:
                info += "���סG����ͭx�̫��";
                break;
            case SpellId.grassCollide:
                info += "�����G�y�� 6 �I�ˮ`";
                break;
            case SpellId.waterCollide:
                info += "�����G�y�� 4 �I�ˮ`";
                break;
            case SpellId.fireCollide:
                info += "�����Q�R�G�y�� 20 �I�ˮ`";
                break;
            case SpellId.slime:
                info += "�H�G�G���� 1 �H�G�A�b���a�������Ѥ��ͦ��H�G";
                break;
        }

        info += "\n";


        switch (player.playerId)
        {
            case "green_slime":
                info += "�۵M�^�_�G�줸���Q����ɡA�^�_ 1 �I�ͩR";
                break;
            case "blue_slime":
                info += "�H�G�`���G�������Q����ɡA�W�[ 1 �H�G";
                break;
            case "red_slime":
                info += "�����E���G�������Q����ɡA���C�����Q�R 1 ��ǳ�";
                break;
        }

        description.text = info;
    }
}
