using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderHandle : ManagerDefender
{
    public float d_returnSpeed = 2.0f;
    public float d_detectionRange = 0.35f;
    public GameObject parent;
    public GameObject collide;
    public Material[] d_materials;
    GameObject target;
    Vector3 lookAtTarget;
    Vector3 prePos;
    Quaternion rotDefender;
    [HideInInspector]
    public bool isDetection = false;
    float timeLimitCount = 0f;
    bool noDetectToBack = false;
    bool isReturning = false;
    bool collided = false;
    
    string nameTag = "";
    //bool noDetected = false;
    private void Start()
    {
        prePos = new Vector3(GameObject.Find("Plane").GetComponent<RoundManager>().point.x, 
            0, GameObject.Find("Plane").GetComponent<RoundManager>().point.z); 
    }
    void DetectCarryingAttacker()
    {
        if (collide.GetComponent<FollowObj>().isDetect == true && isDetection == false && isReturning == false)
        {
            //Debug.Log("a");
            target = GameObject.FindWithTag("CarryingAttacker");
            nameTag = "CarryingAttacker";
            isDetection = true;
            isReturning = true;
            collide.GetComponent<FollowObj>().isDetect = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cooldown")
        {
            isDetection = false;
            isReturning = true;
            collided = true;
            target = null;
        }
    }

    private void Update()
    {
        // have target checking or not?
        if (target == null)
        {
            DetectCarryingAttacker();
        }
        else if (target != null)
        {
            if (target.gameObject.tag == nameTag)
            {
                Move(target.transform.position, t_normalSpeed);
            }
        }
        if(collided == true && target == null)
        {
            if (isReturning == true && isDetection == false)
            {
                Move(prePos, d_returnSpeed);
            }
        }
        else if(target != null && collided == false)
        {
            if(target.gameObject.tag != nameTag)
            {
                Move(prePos, d_returnSpeed);
                if (transform.position.x == prePos.x && transform.position.z == prePos.z)
                {
                    t_detecter.SetActive(false);
                    target = null;
                    isReturning = true;
                    noDetectToBack = true;
                }
            }
        }

        if (target == null && isReturning == true && noDetectToBack == true)
        {
            Debug.Log("no detect");
            transform.tag = "Defender";
            isReturning = false;
            isDetection = false;
            target = null;
            nameTag = "";
            noDetectToBack = false;
        }
        if (transform.position.x == prePos.x && transform.position.z == prePos.z && /*target == null && coolDown &&*/ collided == true)
        {
            t_detecter.SetActive(false);
            transform.GetComponent<MeshRenderer>().material = d_materials[0];
            transform.gameObject.layer = 11;
            collided = false;
            target = null;
        }
        if (transform.gameObject.layer == 11 && collided == false)
        {
            CoolDownTime();
        }
    }
    void Move(Vector3 v, float s)
    {
        lookAtTarget = new Vector3(v.x - transform.position.x, 0, v.z - transform.position.z);

        if(lookAtTarget != new Vector3(0, 0, 0))
        {
            rotDefender = Quaternion.LookRotation(lookAtTarget);
        }

        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y,
            transform.position.z), new Vector3(v.x, transform.position.y, v.z), s * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotDefender, 20f * Time.deltaTime);

        t_detecter.SetActive(true);
    }

    void CoolDownTime()
    {
        timeLimitCount += Time.deltaTime;
        
        if (timeLimitCount >= t_reactivateTime)
        {
            timeLimitCount = 0f;
            transform.gameObject.layer = 9;
            transform.GetComponent<MeshRenderer>().material = d_materials[1];
            transform.tag = "Defender";
            isReturning = false;
            isDetection = false;
            target = null;
            nameTag = "";
            Debug.Log(target);
            Debug.Log(nameTag);
        }
        else return;
    }
}
