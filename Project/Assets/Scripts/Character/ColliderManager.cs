using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [HideInInspector]
    public bool isDetect = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CarryingAttacker" && transform.parent.GetComponent<DefenderHandle>().isDetection == false && isDetect == false)
        {
            //Debug.Log("a");
            isDetect = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "CarryingAttacker" && transform.parent.GetComponent<DefenderHandle>().isDetection == false && isDetect == false)
        {
            Debug.Log("b");
            isDetect = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CarryingAttacker" && transform.parent.GetComponent<DefenderHandle>().isDetection == false && isDetect == false)
        {
            //Debug.Log("c");
            isDetect = true;
        }
    }
}
