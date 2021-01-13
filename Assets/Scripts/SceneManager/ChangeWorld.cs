using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWorld : MonoBehaviour
{
    public WorldState state;
    private AudioSource switchOn, switchOff;
    public AudioClip on, off;
    void Start()
    {
        switchOn = gameObject.AddComponent<AudioSource>();
        switchOff = gameObject.AddComponent<AudioSource>(); 
        switchOn.clip = on;
        switchOff.clip = off;
        state.UpdateWorldObjects();
    }
    void Update()
    {
        if(Input.GetKeyDown("f"))
        {
            state.Flip();
            if(state.GetState() == State.Light)
            {
                switchOff.Play();
            }
            else
            {
                switchOn.Play();
            }
        }
    }
}
