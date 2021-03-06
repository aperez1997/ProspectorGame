using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Utilities for the user interface
/// </summary>
public class GUIUtils 
{
    /// <summary>
    /// Update cost for an action button
    /// </summary>
    public static void UpdateActionButtonCost(TextMeshProUGUI costText, bool allowed, int cost)
    {
        costText.text = allowed ? cost.ToString() + " AP" : String.Empty;
    }

    /// <summary>
    /// Update chance text for an action button
    /// </summary>
    public static void UpdateActionButtonChance(TextMeshProUGUI chanceText, int chance)
    {
        if (chance > 0) {
            chanceText.gameObject.SetActive(true);
            chanceText.text = chance.ToString() + "%";
            var color = Color.red;
            if (chance > 66) {
                color = Color.green;
            } else if (chance > 33) {
                color = Color.yellow;
            }
            chanceText.color = color;
        } else {
            chanceText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Set state of an action button for items. This is based on the check
    /// </summary>
    public static void UpdateItemActionButtonState(TonyButton button, TextMeshProUGUI textCost, ItemActionCheck check)
    {
        Debug.Log(check is ItemActionCheck ? "check " + check.ToString() : "null");
        if (check.IsApplicableToItem) {
            // update the cost
            GUIUtils.UpdateActionButtonCost(textCost, true, check.Cost.Sum);

            if (check.IsAble) {
                button.interactable = true;
            } else {
                button.SetInteractableButDisabled();
            }

            button.gameObject.SetActive(true);
        } else {
            // if not applicable, hide it
            button.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Get then display string for the given sumDescription object
    /// </summary>
    public static string GetSumDescriptionDisplayString(SumDescription sumDescription)
    {
        return sumDescription.GetDescriptionText("\n");
    }

    /// <summary>
    /// Gets the string to display the given amount of money 
    /// </summary>
    /// <param name="amount">Number of cents</param>
    /// <returns>formatted string</returns>
    public string GetMoneyDisplay(int amount)
    {
        var dollars = amount / 100;
        var cents = amount % 100;
        var output = "$" + dollars;
        if (cents > 0) {
            output += "." + cents;
        }
        return output;
    }
}
