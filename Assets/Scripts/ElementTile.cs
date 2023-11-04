using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ElementTile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int xGrid, yGrid;

    [SerializeField] private UnityEvent<int, int> _leftClicked;
    [SerializeField] private UnityEvent<int, int> _rightClicked;
    [SerializeField] private UnityEvent<int, int> _entered;
    [SerializeField] private UnityEvent<int, int> _exited;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _leftClicked.Invoke(xGrid, yGrid);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            _rightClicked.Invoke(xGrid, yGrid);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _entered.Invoke(xGrid, yGrid);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _exited.Invoke(xGrid, yGrid);
    }
}
