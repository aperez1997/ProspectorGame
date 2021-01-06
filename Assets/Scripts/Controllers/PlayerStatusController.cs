using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays player status effects in a container using prefabs
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    public GameObject ItemTemplate;

    protected Transform ItemContainer;

    private PlayerStatus PlayerStatus;

    protected virtual void Awake()
    {
        ItemContainer = Utils.FindInChildren(gameObject, "Container").transform;
        ItemTemplate.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerStatus = GameStateManager.LogicInstance.Player.Status;
        PlayerStatus.OnStatusEffectsChange += Status_OnStatusEffectsChanged;
        UpdateUI();
    }

    public void UpdateUI()
    {
        Debug.Log("player has " + PlayerStatus.EffectsList.Count + " effects");

        // remove old display
        foreach (Transform child in ItemContainer) {
            // don't destroy the Template or weird things happen
            if (child == ItemTemplate.transform) { continue; }
            Destroy(child.gameObject);
        }

        // display items   
        foreach (PlayerStatusEffect psEffect in PlayerStatus.EffectsList) {
            GameObject goItem = Instantiate(ItemTemplate, ItemContainer);
            SetPrefab(goItem, psEffect);
            goItem.SetActive(true);
        }
    }

    public static void SetPrefab(GameObject gameObject, PlayerStatusEffect playerStatusEffect)
    {
        ToolTipUIHelper helper = gameObject.GetComponent<ToolTipUIHelper>();
        helper.text = playerStatusEffect.Description;
    }

    protected void Status_OnStatusEffectsChanged(object sender, EventArgs e)
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        // if we don't do this, it will still get called and cause errors
        // because the gameObjects will be gone
        PlayerStatus.OnStatusEffectsChange -= Status_OnStatusEffectsChanged;
    }
}
