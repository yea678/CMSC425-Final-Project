using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldTexture : MonoBehaviour
{
    public Texture lightTex;
    public Texture darkTex;
    public WorldState currState;
    public void UpdateTexture()
    {
        if(currState.GetState() == State.Light)
        {
            GetComponent<RawImage>().texture = lightTex;
        }
        else
        {
            GetComponent<RawImage>().texture = darkTex;
        }
    }
}
