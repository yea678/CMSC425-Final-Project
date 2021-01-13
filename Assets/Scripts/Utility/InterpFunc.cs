using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpFuncs
{
    private static float tau = Mathf.PI*2;

    public static float TrigFunc(float x)
    {
        return (-1/tau)*(Mathf.Cos((tau*x) - (Mathf.PI/2))) + x;
    }

    public static float HalfTrigFunc(float x)
    {
        return ( (Mathf.Cos(Mathf.PI*x)) / (-2f) ) + 0.5f;
    }

    public static float LinearFunc(float x)
    {
        return x;
    }
}
