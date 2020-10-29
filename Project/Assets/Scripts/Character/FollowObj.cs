using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    public GameObject FollowThisObj;
    [HideInInspector]
    public bool isDetect = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CarryingAttacker" && FollowThisObj.GetComponent<DefenderHandle>().isDetection == false && isDetect == false)
        {
            isDetect = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "CarryingAttacker" && FollowThisObj.GetComponent<DefenderHandle>().isDetection == false && isDetect == false)
        {
            isDetect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CarryingAttacker" && FollowThisObj.GetComponent<DefenderHandle>().isDetection == false && isDetect == false)
        {
            isDetect = true;
        }
    }
    private void FixedUpdate()
    {
        transform.position = FollowThisObj.transform.position;
    }
}
