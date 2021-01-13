using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGoal : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == player)
        {
#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
