using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Maintains current status of player, specifically temporary Status Effects
/// </summary>
[Serializable]
public class PlayerStatus
{
    // Effects
    [SerializeField] private List<PlayerStatusEffect> _effectsList;
    public ReadOnlyCollection<PlayerStatusEffect> EffectsList { get { return _effectsList.AsReadOnly(); } }

    // This is how we emit that the Effects have changed. UI will catch this and update
    public event EventHandler<EventArgs> OnStatusEffectsChange;

    public PlayerStatus()
    {
        _effectsList = new List<PlayerStatusEffect>();
    }

    public bool HasEffect(String name)
    {
        return HasEffect(name, out _);
    }

    public bool HasEffect(String id, out PlayerStatusEffect psEffect)
    {
        psEffect = _effectsList.Find(x => x.Id == id);
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
            Debug.Log("effect re-up " + pStatusEffect.ToString());
            // just re-up the duration
            pStatusEffect.DaysLeft = durationDays;
        } else {
            pStatusEffect = new PlayerStatusEffect(key, durationDays);
            Debug.Log("new effect " + pStatusEffect.ToString());
            _effectsList.Add(pStatusEffect);
        }
        OnStatusEffectsChange?.Invoke(this, EventArgs.Empty);
        return pStatusEffect;
    }

    public void RemoveEffect(String id)
    {
        var removeList = _effectsList.FindAll(x => x.Id == id);
        RemoveEffectList(removeList);
    }

    public void RemoveEffectList(List<PlayerStatusEffect> removeList)
    {
        foreach (var removeEffect in removeList) {
            Debug.Log("effect remove " + removeEffect.ToString());
            _effectsList.Remove(removeEffect);
        }
        OnStatusEffectsChange?.Invoke(this, EventArgs.Empty);
    }

    public ReadOnlyCollection<PlayerStatusEffect> GetEffectsThatInfluenceStat(PlayerStat stat)
    {
        List<PlayerStatusEffect> psEffects = new List<PlayerStatusEffect>();
        foreach (var psEffect in _effectsList) {
            if (psEffect.AffectedStat == stat) {
                psEffects.Add(psEffect);
            }
        }
        return psEffects.AsReadOnly();
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
