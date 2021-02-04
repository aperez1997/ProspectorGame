using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Very architectual SO for establishing costs and assigning events to actions
/// </summary>
[CreateAssetMenu(fileName = "Action", menuName = "ActionData")]
public class ActionData : ScriptableObject
{
    public ActionType type;

    [Tooltip("Cost")]
    public int ActionPointCost;

    [Tooltip("Events that can trigger when this action occurs")]
    public GameEvent[] gameEvents;
}
