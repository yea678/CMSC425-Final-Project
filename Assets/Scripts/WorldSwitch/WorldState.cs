using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Light,
    Dark
}
[CreateAssetMenu(menuName = "ScriptableObjects/WorldState", order = 1)]
public class WorldState : ScriptableObject
{
    public State initState;
    public State currState;

    private void OnEnable()
    {
        currState = initState;
    }

    public State GetState()
    {
        return currState;
    }

    public void Flip()
    {
        if(currState == State.Light)
        {
            currState = State.Dark;
        }
        else
        {
            currState = State.Light;
        }
        
        UpdateWorldObjects();
    }

    public void UpdateWorldObjects()
    {
        WorldMaterial[] matsToUpdate = GameObject.FindObjectsOfType<WorldMaterial>();
        foreach (WorldMaterial item in matsToUpdate)
        {
            item.UpdateMaterial();
        }
        WorldTexture[] texsToUpdate = GameObject.FindObjectsOfType<WorldTexture>();
        foreach (WorldTexture item in texsToUpdate)
        {
            item.UpdateTexture();
        }      
        WorldDependentItem[] objsToUpdate = GameObject.FindObjectsOfType<WorldDependentItem>();
        foreach (WorldDependentItem item in objsToUpdate)
        {
            item.HandleWorldUpdate();
        }       
    }
}
