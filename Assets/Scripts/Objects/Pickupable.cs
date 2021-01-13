using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : WorldSpecificItem
{
    public Player player;
    private Rigidbody rb;
    private bool isHeld;
    private int collidingWith;
    public AudioClip[] hitSFX;
    private AudioSource[] hits;

    public float pickupDistance = 3f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isHeld = false;
        collidingWith = 0;
        hits = new AudioSource[hitSFX.Length];
        for(int i = 0; i < hitSFX.Length; i++)
        {
            hits[i] = gameObject.AddComponent<AudioSource>();
            hits[i].spatialBlend = 1;
            hits[i].volume = 0.15f;
            hits[i].clip = hitSFX[i];
        }

    }

    void OnMouseDown()
    {
        if(!isHeld)
        {
            pickupObject();
        }
        else
        {
            releaseObject();
        }

    }

    private void pickupObject()
    {
        Transform playerTransform = player.transform;
        float distToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        if(distToPlayer <= pickupDistance && currState.GetState() == activeState)
        {
            player.pickup(this);
        }
    }

    private void releaseObject()
    {
        if(isHeld)
        {
            player.drop(this);
        }
    }

    public void setHeld(bool held)
    {
        rb.useGravity = !held;
        isHeld = held;
    }

    public override void HandleWorldUpdate()
    {
        base.HandleWorldUpdate();
        if(isHeld && activeState != currState.GetState())
        {
            releaseObject();
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        hits[Random.Range(0, hits.Length-1)].Play();
        collidingWith++;
    }
    private void OnCollisionExit(Collision c)
    {
        collidingWith--;
    }
    public int touchingColliders()
    {
        return collidingWith;
    }
}
