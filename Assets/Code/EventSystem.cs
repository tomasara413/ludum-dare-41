using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsOver = false;
    }
}
