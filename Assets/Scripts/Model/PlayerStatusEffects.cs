using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Links a StatusEffect SO to a player.
/// keyed by name, adds "days left"
/// </summary>
[Serializable]
public class PlayerStatusEffect
{
    public string Id;

    public int DaysLeft;

    public PlayerStatusEffect(string id, int daysLeft)
    {
        this.Id = id;
        DaysLeft = daysLeft;
    }

    // Lazy-loaded SO object
    private StatusEffect _statusEffect;
    public StatusEffect StatusEffect {
        get {
            if (_statusEffect is null) {
                var data = StatusEffectLoader.LoadItemById(Id);
                if (!(data is StatusEffect)) {
                    throw new System.Exception("No StatusEffect data for ID " + Id);
                }
                _statusEffect = data;
            }
            return _statusEffect;
        }
    }

    // Derived field that come from StatusEffect
    public string Name { get { return StatusEffect.Name; } }

    public string Description { get { return StatusEffect.Description; } }

    public Sprite Sprite { get { return StatusEffect.Sprite; } }

    public PlayerStat AffectedStat { get { return StatusEffect.AffectedStat; } }

    public int AffectAmount { get { return StatusEffect.AffectAmount; } }

    public int DurationDays { get { return StatusEffect.DurationDays; } }

    public override string ToString()
    {
        return "PSE[" + StatusEffect.ToString() + "d:"+DaysLeft + "]";
    }

    public string AffectedAmountNice() {
        var op = AffectAmount > 0 ? "+" : "";
        return op + AffectAmount.ToString();
    } 
}
