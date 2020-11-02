using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Tooltip UI element. Attach to canvas with prefab inside
// use statics to show/hide
public class ToolTipV2 : MonoBehaviour
{
    public static ToolTipV2 Instance { get; private set; }

    public Transform canvasTransform;

    private RectTransform RectTransform;
    private TextMeshProUGUI TextMeshPro;
    private RectTransform BackgoundRectTransform;

    private void Awake()
    {
        Instance = this;

        // save references for performance
        RectTransform = gameObject.GetComponent<RectTransform>();
        TextMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        BackgoundRectTransform = gameObject.GetComponentInChildren<Image>().GetComponent<RectTransform>();

        Hide();
    }

    public void SetText(Transform parentTransform, string text)
    {
        // position
        transform.position = parentTransform.position;

        // TODO? Change pivot based on parent pivot

        // text        
        TextMeshPro.text = text;
        TextMeshPro.ForceMeshUpdate(); // do this before trying to determine BG size

        // render text for BG size
        Vector2 textSize = TextMeshPro.GetRenderedValues(true);
        Vector2 paddingSize = new Vector2(TextMeshPro.margin.x * 2, TextMeshPro.margin.y * 2);

        // change BG size       
        BackgoundRectTransform.sizeDelta = textSize + paddingSize;
    }

    public void Show(Transform parentTransform, string text)
    {
        gameObject.SetActive(true);
        SetText(parentTransform, text);
    }

    public static void Show_Static(Transform parentTransform, string text)
    {
        Instance.Show(parentTransform, text);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void Hide_Static()
    {
        Instance.Hide();
    }
}
