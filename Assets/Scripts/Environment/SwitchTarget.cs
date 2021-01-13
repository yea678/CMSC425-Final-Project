using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwitchTarget : MonoBehaviour
{
    public abstract void UpdateFromSwitch(float interpParam);
}
