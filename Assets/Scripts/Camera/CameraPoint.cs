using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CameraPoint", order = 1)]
public class CameraPoint : ScriptableObject
{
    public float xPos, yPos, zPos, xRot, yRot, zRot, timeToNextPos, timeToNextRot;
}
