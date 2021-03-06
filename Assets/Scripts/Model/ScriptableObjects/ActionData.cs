using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Very architectual SO for establishing costs and assigning events to actions
/// </summary>
[CreateAssetMenu(fileName = "Action", menuName = "ActionData")]
public class ActionData : ScriptableObject, IHasGameEvents
{
    public ActionType type;

    [Tooltip("Cost")]
    public int ActionPointCost;

    [Tooltip("Events that can trigger when this action occurs")]
    public GameEvent[] gameEvents;

    [Tooltip("Used for the button")]
    public string actionName;

    GameEvent[] IHasGameEvents.GetGameEvents(){ return gameEvents; }
}
