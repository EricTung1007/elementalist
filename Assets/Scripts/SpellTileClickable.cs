using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SpellTileClickable : MonoBehaviour, IPointerDownHandler
{
    // The tile number in the grid
    public int tileNumber;
    // Keys for tile selection
    private KeyCode keypad, key;

    [SerializeField] private UnityEvent<int> Select;

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
    }
    public void Update()
    {
        if ( (Input.GetKeyDown(keypad) ) || ( Input.GetKeyDown(key) )){
            Select?.Invoke(tileNumber);
        }
    }
}
