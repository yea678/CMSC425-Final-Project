using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDoor : SwitchTarget
{
    public GameObject leftDoor, rightDoor;
    public float moveAmount = 3f;
    private Vector3 leftInit, rightInit, leftFinal, rightFinal;
    void Start()
    {
        leftInit = leftDoor.transform.localPosition;
        rightInit = rightDoor.transform.localPosition;

        leftFinal = leftDoor.transform.localPosition;
        rightFinal = rightDoor.transform.localPosition;
        leftFinal.x -= moveAmount*transform.localScale.x;
        rightFinal.x += moveAmount*transform.localScale.x;
    }
    public override void UpdateFromSwitch(float interpParam)
    {
        float ipAdjusted = InterpFuncs.TrigFunc(interpParam);
        leftDoor.transform.localPosition = Vector3.Lerp(leftFinal, leftInit, ipAdjusted);
        rightDoor.transform.localPosition = Vector3.Lerp(rightFinal, rightInit, ipAdjusted);
    }
}
