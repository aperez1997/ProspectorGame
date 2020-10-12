using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    public Sprite rationSprite;

    private void Awake()
    {
        Instance = this;
    }
}
