using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bind any scriptble objects that need to be used in the code.
/// For example:
/// - starting equipment / buffs
/// - items given to player on completion of actions
/// </summary>
public class ScriptedObjectBinder : MonoBehaviour
{
    // starting gear
    [Tooltip("Starting buff")]
    public StatusEffect seWellRested;
    [Tooltip("starting gear")]
    public ItemData itemMoney;
    [Tooltip("starting gear")]
    public ItemData itemRation;
    [Tooltip("starting gear")]
    public ItemData itemGoldPan;

    // handouts
    [Tooltip("given on successful foraging")]
    public ItemData itemForagedFood;
    [Tooltip("given on successful gold panning")]
    public ItemData itemGoldNugget;

    /// <summary>
    /// Singleton pattern
    /// </summary>
    public static ScriptedObjectBinder Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
