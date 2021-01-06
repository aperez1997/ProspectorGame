using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedObjectBinder : MonoBehaviour
{
    public StatusEffect seWellRested;

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
