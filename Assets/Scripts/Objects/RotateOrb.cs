using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOrb : MonoBehaviour
{
    private bool shouldAnimate;

    void Start()
    {
        shouldAnimate = true;
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        float rotX = 0, rotY = 0, rotZ = 0;
        
        float avgSpeed = 45;
        float variance = 15;

        float xPer = Random.Range(0.8f, 1.2f), yPer = Random.Range(0.8f, 1.2f), zPer = Random.Range(0.8f, 1.2f);

        float speedT = 0, speedX, speedY, speedZ;
        float speedXSign = 1, speedYSign = 1, speedZSign = 1;
        float lastXSwap = 0, lastYSwap = 0, lastZSwap = 0;

        while(shouldAnimate)
        {
            speedT = (speedT + Time.deltaTime) % (2*Mathf.PI);
            speedX = speedXSign*variance*Mathf.Sin(xPer*speedT) + avgSpeed;
            speedY = speedYSign*variance*Mathf.Cos(yPer*speedT) + avgSpeed;
            speedZ = speedZSign*variance*Mathf.Sin(zPer*speedT + Mathf.PI) + avgSpeed;

            lastXSwap += Time.deltaTime;
            lastYSwap += Time.deltaTime;
            lastZSwap += Time.deltaTime;

            if (speedX <= avgSpeed-(variance/2) && Random.Range(0f, 1f) <= 0.5f && lastXSwap >= 1)
            {
                speedXSign *= -1;
                lastXSwap = 0;
            }
            if (speedY <= avgSpeed-(variance/2) && Random.Range(0f, 1f) <= 0.5f && lastYSwap >= 1)
            {
                speedYSign *= -1;
                lastYSwap = 0;
            }
            if (speedZ <= avgSpeed-(variance/2) && Random.Range(0f, 1f) <= 0.5f && lastZSwap >= 1)
            {
                speedZSign *= -1;
                lastZSwap = 0;
            }

            rotX = (rotX + (Time.deltaTime*speedX)) % 360;
            rotY = (rotY + (Time.deltaTime*speedY)) % 360;
            rotZ = (rotZ + (Time.deltaTime*speedZ)) % 360;
            
            transform.localEulerAngles = new Vector3(rotX, rotY, rotZ);

            yield return null;
        }
    }
}
