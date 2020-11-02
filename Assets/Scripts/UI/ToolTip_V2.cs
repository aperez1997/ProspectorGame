using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// A UI Element that shows a tooltip when moused over
// Tooltip is an instance, so there's only 1 copy of it ever
public class ToolTip_V2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;

    private static GameObject GoToolTip;
    private static RectTransform ToolTipTransform;
    private static TextMeshProUGUI TextMeshPro;
    private static RectTransform BackgoundRectTransform;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show(); // have to show first, or the bg won't size properly 
        SetText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    private void CreateToolTip()
    {
        if (GoToolTip is GameObject) { return; }

        GameObject template = Resources.Load<GameObject>("pfToolTip");

        // put it in the canvas so it won't have heirachry problems
        Transform canvasTransform = transform.GetComponentInParent<Canvas>().transform;
        GoToolTip = Instantiate(template, canvasTransform);

        ToolTipTransform = GoToolTip.GetComponent<RectTransform>();
        TextMeshPro = GoToolTip.GetComponentInChildren<TextMeshProUGUI>();
        BackgoundRectTransform = GoToolTip.GetComponentInChildren<Image>().GetComponent<RectTransform>();
    }

    private void SetText()
    {
        CreateToolTip();

        // position
        ToolTipTransform.position = transform.position;
        // adjust?
        ToolTipTransform.localPosition += new Vector3(0, 2, 0);

        // text        
        TextMeshPro.text = text;
        TextMeshPro.ForceMeshUpdate();

        // render text for BG size
        Vector2 textSize = TextMeshPro.GetRenderedValues(true);
        Vector2 paddingSize = new Vector2(TextMeshPro.margin.x * 2, TextMeshPro.margin.y * 2);

        // change BG size       
        BackgoundRectTransform.sizeDelta = textSize + paddingSize;
    }

    public void Show()
    {
        CreateToolTip();
        GoToolTip.SetActive(true);
    }

    public void Hide()
    {
        CreateToolTip();
        GoToolTip.SetActive(false);
    }
}
