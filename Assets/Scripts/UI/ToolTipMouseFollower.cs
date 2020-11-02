using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Makes the tooltip follow the mouse cursor.
// Courtesy https://www.youtube.com/watch?v=YUIohCXt_pc
public class ToolTipMouseFollower : MonoBehaviour
{
    public static ToolTipMouseFollower Instance { get; private set; }

    public Camera UICamera;

    private RectTransform canvasRectTransform;

    private RectTransform tooltipTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgoundRectTransform;

    private void Awake()
    {
        Instance = this;

        tooltipTransform = GetComponent<RectTransform>();

        Canvas canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();

        backgoundRectTransform = GetComponentInChildren<Image>().GetComponent<RectTransform>();

        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        //SetText("Foo barson asdfasdfads");
        Hide();
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, UICamera, out Vector2 localPosition);

        Vector2 offset = new Vector2(5, 5);

        if (localPosition.x + backgoundRectTransform.rect.width > canvasRectTransform.rect.width){
            // tooltip right side
            localPosition.x = canvasRectTransform.rect.width - backgoundRectTransform.rect.width;
        }

        if (localPosition.y + backgoundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            // tooltip top side
            localPosition.y = canvasRectTransform.rect.height - backgoundRectTransform.rect.height;
        }

        tooltipTransform.localPosition = localPosition + offset;
    }

    private void SetText(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        // TODO: preserve this on awake?
        Vector2 paddingSize = new Vector2(textMeshPro.margin.x * 2, textMeshPro.margin.y * 2);
        backgoundRectTransform.sizeDelta = textSize + paddingSize;
    }

    public void Show(string text)
    {
        gameObject.SetActive(true);
        SetText(text);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void Show_Static(string text)
    {
        Instance.Show(text);
    }

    public static void Hide_Static()
    {
        Instance.Hide();
    }
}
