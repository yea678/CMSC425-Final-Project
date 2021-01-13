using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    public CameraPoint[] points;
    public Camera main;
    private Camera pan;
    public Player player;
    private int index;
    private bool shouldAnimate;
    void Start()
    {
        pan = GetComponent<Camera>();
        index = 0;
        shouldAnimate = true;
        main.enabled = false;
        pan.enabled = true;
        StartCoroutine(AnimateCamera());
    }

    IEnumerator AnimateCamera()
    {       
        float rotInterpParam = 0;
        float transInterpParam = 0;
        float newTransX = 0;
        float newRotX = 0;
        bool isAnimating; 

        while(shouldAnimate)
        {
            CameraPoint startPoint = points[index];
            CameraPoint endPoint = points[index+1];

            Vector3 startPos = new Vector3(startPoint.xPos,startPoint.yPos, startPoint.zPos);
            Vector3 endPos = new Vector3(endPoint.xPos, endPoint.yPos, endPoint.zPos);
            transform.position = startPos;

            Quaternion startRot = Quaternion.Euler(startPoint.xRot,startPoint.yRot, startPoint.zRot);
            Quaternion endRot = Quaternion.Euler(endPoint.xRot,endPoint.yRot, endPoint.zRot);
            transform.localRotation = startRot;

            float transAniTime = startPoint.timeToNextPos;
            float rotAniTime = startPoint.timeToNextRot;
            if(transAniTime <= 0)
            {
                transAniTime = 0.05f;
            }
            if(rotAniTime <= 0)
            {
                rotAniTime = 0.05f;
            }
            
            isAnimating = true;

            while (isAnimating)
            {
                newTransX += (Time.deltaTime / transAniTime);
                newRotX += (Time.deltaTime / rotAniTime);

                rotInterpParam = InterpFuncs.HalfTrigFunc(newRotX);
                transInterpParam = InterpFuncs.LinearFunc(newTransX);

                if (newRotX >= 1)
                {
                    rotInterpParam = 1;
                    newRotX = 1;
                }

                if (newTransX >= 1)
                {
                    transInterpParam = 1;
                    newTransX = 1;
                }

                if (newRotX == 1 && newTransX == 1)
                {
                    isAnimating = false; 
                }

                transform.position = Vector3.Lerp(startPos, endPos, transInterpParam);
                transform.localRotation = Quaternion.Lerp(startRot, endRot, rotInterpParam);
                yield return null;
            }

            rotInterpParam = 0;
            transInterpParam = 0;
            newTransX = 0;
            newRotX = 0;

            index++;

            if(index == points.Length-1)
            {
                shouldAnimate = false;
            }
        }
        
        yield return new WaitForSeconds(0.3f);

        pan.enabled = false;
        main.enabled = true;
        player.enableControls();
    }
}
