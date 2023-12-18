using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SpellTileClickable : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    // The tile number in the grid
    public int tileNumber;
    // Keys for tile selection
    private KeyCode keypad, key;

    [SerializeField] private UnityEvent<int> Select;

    public GameObject infoBox;
    string info;

    // Directly select by left clicking on the spell tile
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Select?.Invoke(tileNumber);
        }
    }
    public void Start()
    {
        // Set the key for selecting this tile
        switch (tileNumber)
        {
            case 1:
                keypad = KeyCode.Keypad1;
                key = KeyCode.Z;
                break;
            case 2:
                keypad = KeyCode.Keypad2;
                key = KeyCode.X;
                break;
            case 3:
                keypad = KeyCode.Keypad3;
                key = KeyCode.C;
                break;
            case 4:
                keypad = KeyCode.Keypad4;
                key = KeyCode.A;
                break;
            case 5:
                keypad = KeyCode.Keypad5;
                key = KeyCode.S;
                break;
            case 6:
                keypad = KeyCode.Keypad6;
                key = KeyCode.D;
                break;
            case 7:
                keypad = KeyCode.Keypad7;
                key = KeyCode.Q;
                break;
            case 8:
                keypad = KeyCode.Keypad8;
                key = KeyCode.W;
                break;
            case 9:
                keypad = KeyCode.Keypad9;
                key = KeyCode.E;
                break;
        }

        infoBox = GameObject.Find("Description");
    }
    public void Update()
    {
        if ( (Input.GetKeyDown(keypad) ) || ( Input.GetKeyDown(key) )){
            Select?.Invoke(tileNumber);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (tileNumber)
        {
            case 0:
                infoBox.GetComponent<InfoBox>().currentHovering = "";
                break;
            case 1:
                infoBox.GetComponent<InfoBox>().currentHovering = "火矢：造成 4 點火屬性傷害";
                break;
            case 2:
                infoBox.GetComponent<InfoBox>().currentHovering = "水彈：造成 4 點水屬性傷害";
                break;
            case 3:
                infoBox.GetComponent<InfoBox>().currentHovering = "木刺：造成 4 點木屬性傷害";
                break;
            case 4:
                infoBox.GetComponent<InfoBox>().currentHovering = "火柱：對敵方全體造成 6 點火屬性傷害";
                break;
            case 5:
                infoBox.GetComponent<InfoBox>().currentHovering = "回復：回復 15 點生命";
                break;
            case 6:
                infoBox.GetComponent<InfoBox>().currentHovering = "毒液：施加 5 秒 [ 中毒 ] \n [ 中毒 ] ：每秒失去 3 點生命";
                break;
            case 7:
                infoBox.GetComponent<InfoBox>().currentHovering = "蒸氣震爆：施加 6 秒 [ 暈眩 ]  \n [ 暈眩 ] ：失去當前意圖且無法行動";
                break;
            case 8:
                infoBox.GetComponent<InfoBox>().currentHovering = "泥沼轉化：對敵方全體施加 10 秒 [ 泥濘 ]  \n [ 泥濘 ] ：無法進行進戰攻擊和位移";
                break;
            case 9:
                infoBox.GetComponent<InfoBox>().currentHovering = "藤蔓拉扯：將最後方的敵人移至最前方";
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.GetComponent<InfoBox>().currentHovering = "";
    }
}
