using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays player status effects in a container using prefabs
/// </summary>
public class PlayerStatusController : ContainerDisplayController<PlayerStatusEffect>
{
    private PlayerStatus PlayerStatus;

    // Start is called before the first frame update
    protected override void Start()
    {
        PlayerStatus = GameStateManager.LogicInstance.Player.Status;
        PlayerStatus.OnStatusEffectsChange += OnItemListChanged;
        base.Start();
    }

    protected override void SetPrefabDetails(GameObject gameObject, PlayerStatusEffect pse)
    {
        ToolTipUIHelper helper = gameObject.GetComponent<ToolTipUIHelper>();
        helper.text = pse.Name + "\n" + pse.Description + "\nDays left: " + pse.DaysLeft; 

        // find sprite
        Image image = gameObject.GetComponentInChildren<Image>();
        image.sprite = pse.Sprite;
        image.enabled = true;
    }

    private void OnDestroy()
    {
        // if we don't do this, it will still get called and cause errors
        // because the gameObjects will be gone
        PlayerStatus.OnStatusEffectsChange -= OnItemListChanged;
    }

    protected override ReadOnlyCollection<PlayerStatusEffect> GetItemList()
    {
        return PlayerStatus.EffectsList;
    }
}
