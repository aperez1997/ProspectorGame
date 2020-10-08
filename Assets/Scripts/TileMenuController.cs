using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show() { gameObject.SetActive(true); }
    public void Hide() { gameObject.SetActive(false); }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
