using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Tooltip UI element. Attach to canvas with prefab inside
// use statics to show/hide
public class ToolTipV2 : MonoBehaviour
{
    // The one we show/hide
    private static ToolTipV2 Instance;

    // The one that is never shown. Instead, it always sits at root so it won't be destroyed
    private static ToolTipV2 Template;

    private RectTransform RectTransform;
    private TextMeshProUGUI TextMeshPro;
    private RectTransform BackgoundRectTransform;

    private void Awake()
    {
        if (Template == null)
        {
            Debug.Log("Assigning Tooltip Template");
            Template = this;
            DontDestroyOnLoad(gameObject);
            this.gameObject.name = "ToolTipV2 Template";

            // hide the template so it doesn't show up
            Hide();

        } else if (Template != this)
        {
            if (Instance == null) {
                Debug.Log("Assigning Tooltip Instance");
                Instance = this;
                this.gameObject.name = "ToolTipV2 Instance";
                Init();
            } else {
                Destroy(gameObject);
            }
        }
    }

    private static void CheckInstance()
    {
        if (Instance == null || Instance.gameObject == null) {
            // we have to show before we instantiate for some reason
            Template.Show();
            Instantiate(Template);
            // hide it again, template should not be seen
            Template.Hide();
        }
    }

    private void Init()
    {
        // save references for performance
        RectTransform = gameObject.GetComponent<RectTransform>();
        TextMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        BackgoundRectTransform = gameObject.GetComponentInChildren<Image>().GetComponent<RectTransform>();
    }

    private void SetText(Transform parentTransform, string text)
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

    private void Show(Transform parentTransform, string text)
    {
        // Move to whatever canvas the parent is in. This prevents issues with multiple scenes
        Canvas canvas = parentTransform.GetComponentInParent<Canvas>();
        gameObject.transform.SetParent(canvas.transform);

        // need to show before we set text
        Show();

        SetText(parentTransform, text);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    public static void Show_Static(Transform parentTransform, string text)
    {
        CheckInstance();
        Instance.Show(parentTransform, text);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void Hide_Static()
    {
        CheckInstance();
        Instance.Hide();
    }
}
