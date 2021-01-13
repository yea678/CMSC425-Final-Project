using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMaterial : MonoBehaviour
{
    public Material lightMat;
    public Material darkMat;
    public WorldState currState;
    public void UpdateMaterial()
    {
        //Debug.Log("Updating material of: " + this.name + " to be: " + currState.GetState());
        if(currState.GetState() == State.Light)
        {
            GetComponent<MeshRenderer>().material = lightMat;
        }
        else
        {
            GetComponent<MeshRenderer>().material = darkMat;
        }
    }
}
