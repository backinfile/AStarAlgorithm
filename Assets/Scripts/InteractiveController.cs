using AStarAlgorithm.Heap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractiveController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public UnityAction<bool> OnSet;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnSet?.Invoke(true);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnSet?.Invoke(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            OnSet?.Invoke(true);
        }
        else if (Input.GetMouseButton(1))
        {
            OnSet?.Invoke(false);
        }
    }

}