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
                infoBox.GetComponent<InfoBox>().currentHovering = "���ڡG�y�� 4 �I���ݩʶˮ`";
                break;
            case 2:
                infoBox.GetComponent<InfoBox>().currentHovering = "���u�G�y�� 4 �I���ݩʶˮ`";
                break;
            case 3:
                infoBox.GetComponent<InfoBox>().currentHovering = "���G�y�� 4 �I���ݩʶˮ`";
                break;
            case 4:
                infoBox.GetComponent<InfoBox>().currentHovering = "���W�G��Ĥ����y�� 6 �I���ݩʶˮ`";
                break;
            case 5:
                infoBox.GetComponent<InfoBox>().currentHovering = "�^�_�G�^�_ 15 �I�ͩR";
                break;
            case 6:
                infoBox.GetComponent<InfoBox>().currentHovering = "�i�j�G�I�[ 15 �� [ �i�j ] \n [ �i�j ] �G����ˮ` x 200%";
                break;
            case 7:
                infoBox.GetComponent<InfoBox>().currentHovering = "�����R��G�����Ĥ褸���W�q�A�òM�Ť�����";
                break;
            case 8:
                infoBox.GetComponent<InfoBox>().currentHovering = "�d�h��ơG��Ĥ����I�[ 10 �� [ �d�� ]  \n [ �d�� ] �G�y���ˮ` x 25%";
                break;
            case 9:
                infoBox.GetComponent<InfoBox>().currentHovering = "�ý��ԧ�G�N�̫�誺�ĤH���̫ܳe��";
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.GetComponent<InfoBox>().currentHovering = "";
    }
}
