using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipUIHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipV2.Show_Static(transform, text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipV2.Hide_Static();
    }
}
