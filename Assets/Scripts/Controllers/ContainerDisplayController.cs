using System;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// A controller that displays a list of items in a container using an item template
/// Preferably, your container has a grid layout and is attached to a scroll view 
/// </summary>
/// <typeparam name="ItemType">Class of the item being displayed</typeparam>
abstract public class ContainerDisplayController<ItemType> : MonoBehaviour
{
    /// <summary>
    /// Bound template for each item. Perhaps this is a prefab.
    /// </summary>
    public GameObject ItemTemplate;

    /// <summary>
    /// container gamne object which holds the displated items
    /// </summary>
    protected Transform ItemContainer;

    protected virtual void Awake()
    {
        // cache the item container for efficiency
        CacheItemContainer();
    }

    /// <summary>
    /// Save (cache) the item container to speed up update
    /// </summary>
    protected virtual void CacheItemContainer()
    {
        ItemContainer = FindInChildren(gameObject, this.GetItemContainerName()).transform;
    }

    /// <summary>
    /// The name of the container gameObject. Override as necessary
    /// </summary>
    protected virtual string GetItemContainerName()
    {
        return "Item Container";
    }

    protected virtual void Start()
    {
        // hide template, we shouldn't see it.
        ItemTemplate.SetActive(false);
        // update the UI now
        UpdateUI();
    }

    /// <summary>
    /// Update the UI based on the current items from GetItemList()
    /// </summary>
    public void UpdateUI()
    {
        // remove old display
        foreach (Transform child in ItemContainer) {
            // don't destroy the Template or weird things happen
            if (child == ItemTemplate.transform) { continue; }
            Destroy(child.gameObject);
        }

        // display current items
        foreach (ItemType item in GetItemList()) {
            // instantiate a new object from the template
            GameObject goItem = Instantiate(ItemTemplate, ItemContainer);
            // populate the new object with details from the item
            SetPrefabDetails(goItem, item);
            // make it active, because the template is inactive
            goItem.SetActive(true);
        }
    }

    /// <summary>
    /// Get the items to be displayed
    /// </summary>
    protected abstract ReadOnlyCollection<ItemType> GetItemList();

    /// <summary>
    /// Set all details into the gameObject created from the template 
    /// </summary>
    /// <param name="goItem">the instantiated template gameObject</param>
    /// <param name="item">the item that is currently being rendered</param>
    protected abstract void SetPrefabDetails(GameObject goItem, ItemType item);

    /// <summary>
    /// Event handler for item change, updates the UI
    /// You can connect this to whatever controls your item list
    /// be sure to remove the handler in destroy() if you do so
    /// </summary>
    protected virtual void OnItemListChanged(object sender, EventArgs e)
    {
        UpdateUI();
    }

    // courtesy https://forum.unity.com/threads/transform-find-doesnt-work.12949/
    public static GameObject FindInChildren(GameObject gameObject, string name)
    {
        foreach (var x in gameObject.GetComponentsInChildren<Transform>())
            if (x.gameObject.name == name)
                return x.gameObject;
        throw new System.Exception("Technically the old version throws an exception if none are found, so I'll do the same here!");
    }
}
