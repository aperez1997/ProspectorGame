using UnityEngine;
using System.Linq;

/// <summary>
/// Loads ItemData ScriptableObjects
/// </summary>
public class ItemDataLoader : AssetLoader<ItemData>
{
    private static ItemDataLoader _instance = null;
    private static readonly object padlock = new object();
    public static ItemDataLoader Instance
    {
        get {
            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new ItemDataLoader();
                }
                return _instance;
            }
        }
    }

    public static ItemData LoadItemById(string id)
    {
        return Instance.LoadByKey(id);
    }

    protected override string GetPath(){ return "GameData/Items"; }
}
