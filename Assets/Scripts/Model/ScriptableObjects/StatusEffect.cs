using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "StatusEffect")]
public class StatusEffect : ScriptableObject, IAssetLoaderItem
{
    public new string name;

    public string Description;

    public PlayerStat AffectedStat;

    public int AffectAmount;

    public int DurationDays;

    public string GetKey()
    {
        return name;
    }

    public override string ToString()
    {
        return "SE(" + name + "," + AffectedStat + "=" + AffectAmount + ")";
    }
}

public enum PlayerStat
{
    Health, ActionPoints
}

