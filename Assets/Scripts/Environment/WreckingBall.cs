using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : WorldSpecificItem
{
    public float maxAngle = 45;
    private Quaternion initRot, finalRot, baseInitRot, baseFinalRot;
    public float animationTime = 0.8f;
    private bool shouldAnimate;
    public GameObject joint1, joint2;
    private Collider[] children;
    private Transform jointTransform0, jointTransform1, jointTransform2;
    public AudioClip swooshSFX;
    private AudioSource swoosh;

    void Awake()
    {

        children = GetComponentsInChildren<Collider>();
        
        initRot = Quaternion.Euler(0, 0, -1*maxAngle);
        finalRot = Quaternion.Euler(0, 0, maxAngle);
        //Top joint should always rotate less than the others
        baseInitRot = Quaternion.Euler(0, 0, (-1*maxAngle/2f));
        baseFinalRot = Quaternion.Euler(0, 0, (maxAngle/2f));
        shouldAnimate = true;
        jointTransform0 = transform;
        jointTransform1 = joint1.transform;
        jointTransform2 = joint2.transform;
        swoosh = gameObject.AddComponent<AudioSource>();
        swoosh.spatialBlend = 1;
        swoosh.clip = swooshSFX;
        StartCoroutine(Animate());
    }


    IEnumerator Animate()
    {
        float interpolationParameter = 0;
        float newX = 0;
        float changeSign = 1;
        bool isAnimating;
        
        while(shouldAnimate)
        {
            isAnimating = true;
            swoosh.Play();
            while (isAnimating)
            {
                newX += (Time.deltaTime * changeSign / animationTime);
                interpolationParameter = InterpFuncs.HalfTrigFunc(newX);
                
                if (newX >= 1 || newX <= 0)
                {
                    interpolationParameter = Mathf.Clamp(interpolationParameter, 0, 1);
                    newX = Mathf.Clamp(newX, 0, 1);
                    isAnimating = false; 
                }

                jointTransform0.localRotation = Quaternion.Lerp(baseInitRot, baseFinalRot, interpolationParameter);
                jointTransform1.localRotation = Quaternion.Lerp(initRot, finalRot, interpolationParameter);
                jointTransform2.localRotation = Quaternion.Lerp(initRot, finalRot, interpolationParameter);
                yield return null;
            }

            changeSign *= -1;
        }
    }

    public override void HandleWorldUpdate()
    {
        base.HandleWorldUpdate();
        if(activeState == State.Dark)
        {
            //Debug.Log("Updating: " + this.name + ": " + (activeState == currState.GetState()).ToString());
        }
        modifyColliders(activeState == currState.GetState());
    }


    private void modifyColliders(bool enable)
    {
        foreach (Collider c in children)
        {
            if(c)
            {
                c.enabled = enable;
            }
        }
    }
}
