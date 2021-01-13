using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            resetAll();
        }
    }

    public static void resetAll()
    {
        Resettable[] objsToReset = GameObject.FindObjectsOfType<Resettable>();
        foreach (Resettable item in objsToReset)
        {
            item.reset();
        }    
    }
}
