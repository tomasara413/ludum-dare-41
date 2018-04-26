using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    public bool OnUI = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUI = false;
    }
}
