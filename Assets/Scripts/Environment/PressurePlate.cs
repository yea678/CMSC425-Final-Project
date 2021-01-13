using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : WorldSpecificItem
{
    public float heightUp = 0.09f;

    public float moveTime = 1f; 

    Vector3 transformUp;
    Vector3 transformDown;

    bool isMoving = false;
    bool isPlateUp = true;

    private HashSet<Pickupable> touching = new HashSet<Pickupable>();
    private float changeSign;
    public SwitchTarget switchObject;

    private void Start()
    {
        transformUp = transform.position;
        transformDown = new Vector3(transformUp.x, transformUp.y-heightUp, transformUp.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Pickupable item = collision.gameObject.GetComponent<Pickupable>();
        if(item)
        {
            touching.Add(item);
            if(item.activeState == activeState && isPlateUp)
            {
                StartCoroutine(MovePlate());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Pickupable item = collision.gameObject.GetComponent<Pickupable>();
        if(item)
        {
            touching.Remove(item);
            if(!touchingObjectOfType(this.activeState) && !isPlateUp)
            {
                StartCoroutine(MovePlate());
            }
        }
    }

    IEnumerator MovePlate()
    {
        if (isMoving)
        {
            changeSign *= -1;
            isPlateUp = !isPlateUp;
            yield break;
        }

        isMoving = true;
        isPlateUp = !isPlateUp;

        float interpolationParameter;
        float newX;

        if (!isPlateUp)
        {
            interpolationParameter = 1;
            newX = 1;
            changeSign = -1;
        }
        else
        {
            interpolationParameter = 0;
            newX = 0;
            changeSign = 1;
        }

        while (isMoving)
        {
            newX += changeSign * Time.deltaTime / moveTime;
            interpolationParameter = InterpFuncs.LinearFunc(newX);

            if (newX >= 1 || newX <= 0)
            {
                newX = Mathf.Clamp(newX, 0, 1);
                interpolationParameter = Mathf.Clamp(interpolationParameter, 0, 1);

                isMoving = false;
            }

            transform.position = Vector3.Lerp(transformDown, transformUp, interpolationParameter);
            switchObject.UpdateFromSwitch(interpolationParameter);
            yield return null;
        }
    }

    public override void HandleWorldUpdate()
    {
        activeState = currState.GetState();
        if((!touchingObjectOfType(activeState) && !isPlateUp) || (touchingObjectOfType(activeState) && isPlateUp))
        {
            StartCoroutine(MovePlate());
        }
    }


    private bool touchingObjectOfType(State s)
    {
        foreach (Pickupable p in touching)
        {
            if(p.activeState == s)
            {
                return true;
            }
        }
        return false;
    }
}
