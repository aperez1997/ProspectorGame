using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// A UI Element that shows a tooltip when moused over
public class ToolTipV1 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;

    private GameObject template;
    private GameObject goToolTip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
        //Invoke(nameof(Hide), 0.7f);
    }

    private void CreateToolTip()
    {
        if (goToolTip is GameObject) { return; }

        template = Resources.Load<GameObject>("pfToolTip");

        // put it in the canvas so it won't have heirachry problems
        Transform canvasTransform = transform.GetComponentInParent<Canvas>().transform;
        goToolTip = Instantiate(template, canvasTransform);

        // position
        RectTransform toolTipTransform = goToolTip.GetComponent<RectTransform>();
        toolTipTransform.position = transform.position;
        // adjust right
        toolTipTransform.localPosition += new Vector3(10, 0, 0);

        // text
        TextMeshProUGUI textMeshPro = goToolTip.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = text;
        textMeshPro.ForceMeshUpdate();

        // render text for BG size
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(textMeshPro.margin.x * 2, textMeshPro.margin.y * 2);

        // change BG size
        RectTransform backgoundRectTransform = goToolTip.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        backgoundRectTransform.sizeDelta = textSize + paddingSize;
    }

    public void Show()
    {
        CreateToolTip();
        goToolTip.SetActive(true);
    }

    public void Hide()
    {
        CreateToolTip();
        goToolTip.SetActive(false);
    }
}
