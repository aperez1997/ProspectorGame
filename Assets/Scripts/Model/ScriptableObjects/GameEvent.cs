using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO for events, which are things that can happen randomly when certain actions are taken 
/// </summary>
[CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent")]
public class GameEvent : ScriptableObject, IAssetLoaderItem
{
    [Tooltip("used internally, must be unique")]
    public string id;

    [Tooltip("shown to the player")]
    public new string name;

    [Tooltip("shown to the player")]
    public string description;

    [Tooltip("0-100, chance that this happens when trigger occurs")]
    public int chance;

    [Tooltip("Status Effect given to player, if any")]
    public StatusEffect statusEffectGiven;

    public string GetKey() { return id; }
}