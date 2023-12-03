using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellDescription : MonoBehaviour
{
    TextMeshProUGUI description;
    private void Awake()
    {
        description = GameObject.Find("Description").GetComponent<TextMeshProUGUI>();
    }
    public void SetDescription(int spellTileNumber)
    {
        switch(spellTileNumber)
        {
            case 0:
                description.text = "";
                break;
            case 1:
                description.text = "火矢：造成 4 點火屬性傷害";
                break;
            case 2:
                description.text = "水彈：造成 4 點水屬性傷害";
                break;
            case 3:
                description.text = "木刺：造成 4 點木屬性傷害";
                break;
            case 4:
                description.text = "火柱：對敵方全體造成 4 點火屬性傷害";
                break;
            case 5:
                description.text = "回復：回復 7 點生命";
                break;
            case 6:
                description.text = "毒液：施加 10 秒 [ 中毒 ] ";
                break;
            case 7:
                description.text = "蒸氣震爆：施加 6 秒 [ 暈眩 ] ";
                break;
            case 8:
                description.text = "泥沼轉化：施加 10 秒 [ 泥濘 ] ";
                break;
            case 9:
                description.text = "藤蔓拉扯：將最後方的敵人移至最前方";
                break;
        }
    }
}
