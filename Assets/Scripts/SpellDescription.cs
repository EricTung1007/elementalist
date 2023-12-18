using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellDescription : MonoBehaviour
{
    [SerializeField] GameObject infoBox;
    public void SetDescription(int spellTileNumber)
    {
        
        switch(spellTileNumber)
        {
            case 0:
                infoBox.GetComponent<InfoBox>().currentSelecting = "";
                break;
            case 1:
                infoBox.GetComponent<InfoBox>().currentSelecting = "火矢：造成 4 點火屬性傷害";
                break;
            case 2:
                infoBox.GetComponent<InfoBox>().currentSelecting = "水彈：造成 4 點水屬性傷害";
                break;
            case 3:
                infoBox.GetComponent<InfoBox>().currentSelecting = "木刺：造成 4 點木屬性傷害";
                break;
            case 4:
                infoBox.GetComponent<InfoBox>().currentSelecting = "火柱：對敵方全體造成 6 點火屬性傷害";
                break;
            case 5:
                infoBox.GetComponent<InfoBox>().currentSelecting = "回復：回復 15 點生命";
                break;
            case 6:
                infoBox.GetComponent<InfoBox>().currentSelecting = "綑綁：施加 15 秒 [ 綑綁 ] \n [ 綑綁 ] ：受到傷害 x 200%";
                break;
            case 7:
                infoBox.GetComponent<InfoBox>().currentSelecting = "元素充抵：消除敵方元素增益，並清空元素槽";
                break;
            case 8:
                infoBox.GetComponent<InfoBox>().currentSelecting = "泥沼轉化：對敵方全體施加 10 秒 [ 泥濘 ]  \n [ 泥濘 ] ：造成傷害 x 25%";
                break;
            case 9:
                infoBox.GetComponent<InfoBox>().currentSelecting = "藤蔓拉扯：將最後方的敵人移至最前方";
                break;
        }
    }
}
