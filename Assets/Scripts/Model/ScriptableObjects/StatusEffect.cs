using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "StatusEffect")]
public class StatusEffect : ScriptableObject, IAssetLoaderItem
{
    [Tooltip("Must be unique!")]
    public string Id;

    [Tooltip("display name")]
    public string Name;

    [Tooltip("describe it")]
    public string Description;

    [Tooltip("which stat is affected")]
    public PlayerStat AffectedStat;

    [Tooltip("by how much (positive for buff, negative for debuff)")]
    public int AffectAmount;

    [Tooltip("for how long")]
    public int DurationDays;

    public string GetKey()
    {
        return Id;
    }

    public override string ToString()
    {
        return "SE(" + Id + ","+ Name + "," + AffectedStat + "=" + AffectAmount + ")";
    }
}

public enum PlayerStat
{
    Health, ActionPoints
}

