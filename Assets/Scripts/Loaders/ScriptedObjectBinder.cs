using System;
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
    [Tooltip("ActionData SO for Move Action")]
    public ActionData MoveData;
    [Tooltip("ActionData SO for Forage Action")]
    public ActionData ForageData;
    [Tooltip("ActionData SO for Skin Action")]
    public ActionData SkinningData;
    [Tooltip("ActionData SO for Cook Action")]
    public ActionData CookData;

    [SerializeField]
    [Tooltip("starting equipment")]
    public StartingGear StartingGear;

    // Game Logic related
    [Tooltip("needed for money checks")]
    public ItemData itemMoney;
    [Tooltip("Panning check, should be changed to an item attribute or sub-class")]
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

/// <summary>
/// Gear and Effects that the player starts with
/// </summary>
[Serializable]
public class StartingGear
{
    public ItemDataQuantity[] Items;
    public StatusEffect[] Effects;
}

/// <summary>
/// An item that the player starts, along with the quantity
/// Can't use InventoryItem because that has an ID instead of the SO
/// </summary>
[Serializable]
public class ItemDataQuantity
{
    public ItemData Item;
    public int quantity;
}