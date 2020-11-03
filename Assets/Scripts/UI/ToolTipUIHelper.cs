using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// For UI Elements that have tooltips. Just set the text
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
