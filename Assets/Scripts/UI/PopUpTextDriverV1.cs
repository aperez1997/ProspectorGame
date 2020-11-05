using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to create PopUpTextV1, which is text that floats up and disapears after a few seconds
/// Since it has singleton behavior but creates objects, it can't live in one class
/// </summary>
public class PopUpTextDriverV1 : MonoBehaviour
{
    private static PopUpTextDriverV1 Instance;

    public static PopUpTextV1 CreateSuccessPopUp(Transform parentTransform)
    {
        var text = "Success!";
        PopUpTextV1 popUpTextV1 = CreatePopUp(parentTransform, text);
        popUpTextV1.SetSuccessColor();
        return popUpTextV1;
    }

    public static PopUpTextV1 CreateFailurePopUp(Transform parentTransform)
    {
        var text = "Failed!";
        PopUpTextV1 popUpTextV1 = CreatePopUp(parentTransform, text);
        popUpTextV1.SetFailureColor();
        return popUpTextV1;
    }

    public static PopUpTextV1 CreateSuccessFailurePopUp(Transform parentTransform, bool rv)
    {
        return rv ? CreateSuccessPopUp(parentTransform) : CreateFailurePopUp(parentTransform);
    }

    public static PopUpTextV1 CreateStatChangePopUp(Transform parentTransform, int delta)
    {
        var text = delta.ToString();
        if (delta > 0) { text = "+" + delta; }
        PopUpTextV1 popUpTextV1 = CreatePopUp(parentTransform, text);
        popUpTextV1.SetStatChangeColor(delta);
        return popUpTextV1;
    }

    public static PopUpTextV1 CreateInventoryChangePopUp(Transform parentTransform, int delta, ItemType type)
    {
        var text = delta.ToString();
        if (delta > 0) { text = "+" + delta; }
        text += " " + type.ToString();
        if (delta > 1) { text += "s"; }

        PopUpTextV1 popUpTextV1 = CreatePopUp(parentTransform, text);
        popUpTextV1.SetStatChangeColor(delta);
        return popUpTextV1;
    }

    public static PopUpTextV1 CreatePopUp(Transform parentTransform, string text)
    {
        Transform popUpTransform = Instantiate(Instance.template, parentTransform.transform.position, Quaternion.identity);
        PopUpTextV1 popUpTextV1 = popUpTransform.GetComponent<PopUpTextV1>();
        popUpTextV1.SetText(text);
        popUpTextV1.MoveToParent(parentTransform);
        return popUpTextV1;
    }

    public Transform template;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
