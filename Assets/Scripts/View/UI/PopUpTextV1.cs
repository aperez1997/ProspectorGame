using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controlls a text PopUp that floats up and disappears after a few seconds
/// </summary>
public class PopUpTextV1 : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private float disappearTimer = 1f;

    private void Awake()
    {
        textMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void MoveToParent(Transform parentTransform)
    {
        Canvas canvas = parentTransform.GetComponentInParent<Canvas>();
        transform.SetParent(canvas.transform);

        // set scale to 1 (seems to get mussed when moving it)
        var popUpRectTransform = gameObject.GetComponent<RectTransform>();
        popUpRectTransform.localScale = new Vector3(1, 1, 1);

        // resize object based on actual text size
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        popUpRectTransform.sizeDelta = textSize;

        // check canvas bounds
        var canvasRectTransform = canvas.GetComponent<RectTransform>();
        var anchoredPos = popUpRectTransform.anchoredPosition;

        if (popUpRectTransform.anchoredPosition.x + popUpRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            // off right of screen
            anchoredPos.x = canvasRectTransform.rect.width - popUpRectTransform.rect.width;
        }

        if (popUpRectTransform.anchoredPosition.y + popUpRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            // off top of screen
            anchoredPos.y = canvasRectTransform.rect.height - popUpRectTransform.rect.height;
        }
        popUpRectTransform.anchoredPosition = anchoredPos;
    }

    private void Update()
    {
        float moveSpeed = 30f;
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            // start disappearing
            float disappearSpeed = 3f;
            textMeshPro.alpha -= disappearSpeed * Time.deltaTime;
            if (textMeshPro.alpha < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetInventoryChangeColor(int delta)
    {
        SetStatChangeColor(delta);
    }

    public void SetStatChangeColor(int delta)
    {
        textMeshPro.color = delta > 0 ? Color.green : Color.red;
    }

    public void SetSuccessColor()
    {
        textMeshPro.color = Color.green;
    }

    public void SetFailureColor()
    {
        textMeshPro.color = Color.red;
    }


    public void SetText(string newText)
    {
        textMeshPro.SetText(newText);
        gameObject.name = "PopUpV1 " + newText;
    }
}
