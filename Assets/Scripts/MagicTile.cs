using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SkillTile : MonoBehaviour, IPointerDownHandler
{
    public int number;

    [SerializeField] private UnityEvent<int> _selected;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _selected.Invoke(number);
        }
    }
    public void Update()
    {
        switch (number)
        {
            case 1:
                if (Input.GetKeyDown(KeyCode.Keypad1)) _selected.Invoke(number); break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Keypad2)) _selected.Invoke(number); break;
            case 3:
                if (Input.GetKeyDown(KeyCode.Keypad3)) _selected.Invoke(number); break;
            case 4:
                if (Input.GetKeyDown(KeyCode.Keypad4)) _selected.Invoke(number); break;
            case 5:
                if (Input.GetKeyDown(KeyCode.Keypad5)) _selected.Invoke(number); break;
            case 6:
                if (Input.GetKeyDown(KeyCode.Keypad6)) _selected.Invoke(number); break;
            case 7:
                if (Input.GetKeyDown(KeyCode.Keypad7)) _selected.Invoke(number); break;
            case 8:
                if (Input.GetKeyDown(KeyCode.Keypad8)) _selected.Invoke(number); break;
            case 9:
                if (Input.GetKeyDown(KeyCode.Keypad9)) _selected.Invoke(number); break;
        }
    }
}
