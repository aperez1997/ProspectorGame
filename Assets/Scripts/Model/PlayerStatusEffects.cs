using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Links a StatusEffect SO to a player.
/// keyed by name, adds "days left"
/// </summary>
public class PlayerStatusEffect
{
    public string Name;

    public int DaysLeft;

    public PlayerStatusEffect(string name, int daysLeft)
    {
        this.Name = name;
        DaysLeft = daysLeft;
    }

    // Lazy-loaded SO object
    private StatusEffect _statusEffect;
    public StatusEffect StatusEffect {
        get {
            if (_statusEffect is null) {
                var data = StatusEffectLoader.LoadItemById(Name);
                if (!(data is StatusEffect)) {
                    throw new System.Exception("No StatusEffect data for ID " + Name.ToString());
                }
                _statusEffect = data;
            }
            return _statusEffect;
        }
    }

    // Derived field that come from StatusEffect
    public string Description { get { return StatusEffect.Description; } }

    public PlayerStat AffectedStat { get { return StatusEffect.AffectedStat; } }

    public int AffectAmount { get { return StatusEffect.AffectAmount; } }

    public int DurationDays { get { return StatusEffect.DurationDays; } }
}
