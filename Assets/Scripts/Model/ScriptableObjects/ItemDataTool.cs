using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item that is used to perform actions
/// </summary>
[CreateAssetMenu(fileName = "Tool-", menuName = "Item: Tool")]
public class ItemDataTool : ItemData
{
    public override ItemCategory category { get { return ItemCategory.Tools; } }

    public ToolAbility[] Abilities;

    /// <summary>
    /// return true if this tool can do the given job.
    /// </summary>
    public bool HasAbility(ActionType actionType)
    {
        return HasAbility(actionType, out _);
    }

    /// <summary>
    /// return true if this tool can do the given job.
    /// modifier is return in pbr param
    /// </summary>
    public virtual bool HasAbility(ActionType actionType, out int modifier)
    {
        modifier = 0;
        foreach (var Cap in Abilities) {
            if (Cap.ActionType == actionType) {
                modifier = Cap.modifier;
                return true;
            }
        }
        return false;
    }
}

/// <summary>
/// Action + modifier, assign to a tool to give it abilities
/// </summary>
[System.Serializable]
public class ToolAbility
{
    public ActionType ActionType;
    public int modifier;

    public ToolAbility(ActionType actionType, int modifier)
    {
        ActionType = actionType;
        this.modifier = modifier;
    }
}