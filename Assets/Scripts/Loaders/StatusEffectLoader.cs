using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads StatusEffect SOs
/// </summary>
public class StatusEffectLoader : AssetLoader<StatusEffect>
{
    private static StatusEffectLoader _instance = null;
    private static readonly object padlock = new object();
    public static StatusEffectLoader Instance {
        get {
            lock (padlock) {
                if (_instance == null) {
                    _instance = new StatusEffectLoader();
                }
                return _instance;
            }
        }
    }

    public static StatusEffect LoadItemById(string id)
    {
        return Instance.LoadByKey(id.ToString());
    }

    protected override string GetPath() { return "GameData/StatusEffects"; }
}
