using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains current status of player, specifically temporary Status Effects
/// </summary>
[Serializable]
public class PlayerStatus
{
    public Player player;

    // Effects
    [field: SerializeField] public Dictionary<string, PlayerStatusEffect> PlayerStatusEffects { get; private set; }

    public PlayerStatus(Player player)
    {
        this.player = player;
        PlayerStatusEffects = new Dictionary<string, PlayerStatusEffect>();
    }

    public int GetMaxActionPoints()
    {
        // AP reduced by health loss.
        int healthLossAPRedux = -1 * (Player.MAX_HEALTH - player.Health);

        // effect loss
        int totalEffectChange = GetTotalStatEffect(PlayerStat.ActionPoints);

        // don't go below 1
        return Math.Max(1, player.ActionPointsMax + totalEffectChange - healthLossAPRedux);
    }

    public bool HasEffect(String name)
    {
        return HasEffect(name, out _);
    }

    public bool HasEffect(String name, out PlayerStatusEffect psEffect)
    {
        _ = PlayerStatusEffects.TryGetValue(name, out psEffect);
        return psEffect is PlayerStatusEffect;
    }

    public void AddEffect(String name)
    {
        var data = StatusEffectLoader.LoadItemById(name);
        if (data is StatusEffect) {
            AddEffect(data.GetKey());
        }
    }

    public PlayerStatusEffect AddEffect(StatusEffect effect)
    {
        string key = effect.GetKey();
        int durationDays = effect.DurationDays;
        PlayerStatusEffect pStatusEffect;
        if (HasEffect(key, out pStatusEffect)) {
            // just re-up the duration
            pStatusEffect.DaysLeft = durationDays;
        } else {
            pStatusEffect = new PlayerStatusEffect(key, durationDays);
        }
        return pStatusEffect;
    }

    public void RemoveEffect(String name)
    {
        PlayerStatusEffects.Remove(name);
    }

    public List<PlayerStatusEffect> GetEffectsThatInfluenceStat(PlayerStat stat)
    {
        List<PlayerStatusEffect> psEffects = new List<PlayerStatusEffect>();
        foreach (var tuple in PlayerStatusEffects) {
            var psEffect = tuple.Value;
            if (psEffect.AffectedStat == stat) {
                psEffects.Add(psEffect);
            }
        }
        return psEffects;
    }

    public int GetTotalStatEffect(PlayerStat stat)
    {
        int total = 0;
        var psEffects = GetEffectsThatInfluenceStat(stat);
        foreach (var effect in psEffects) {
            total += effect.AffectAmount;
        }
        return total;
    }
}
