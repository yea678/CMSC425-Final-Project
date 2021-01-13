using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resettable : MonoBehaviour
{
    private Vector3 pos;
    private Quaternion rot;
    void Start()
    {
        pos = transform.position;
        rot = transform.rotation;
    }

    public void reset()
    {
        transform.position = pos;
        transform.rotation = rot;
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
