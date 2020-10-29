using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSolve : MonoBehaviour
{
    public List<Transform> players = new List<Transform>();
    public ParticleSystem boom;
    public GameObject attacker;
    public float speedBall = 1.5f;
    [HideInInspector]
    public bool isGoal = false;
    [HideInInspector]
    public bool isLose = false;
    [HideInInspector]
    public bool collide = false;
    bool isMove = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gate")
        {
            //Debug.Log(isGoal);
            isGoal = true;
            transform.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Attacker")
        {
            isMove = false;
        }
        if (other.gameObject.tag == "Fence")
        {
            isMove = true;
            collide = true;
        }
    }

    private void MoveBall(Transform posPlayer)
    {
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y,
            transform.position.z), new Vector3(posPlayer.position.x, transform.position.y, posPlayer.position.z), speedBall * Time.deltaTime);
    }

    private int FindPositionWithNearestObj()
    {
        int nearest = 0;
        float x = 0;
        if (players.Count > 0)
        {
            x = Vector3.Distance(players[0].position, transform.position);
        }

        for (int i = 1; i < players.Count; i++)
        {
            float tmp = Vector3.Distance(players[i].position, transform.position);
            if (x > tmp)
            {
                x = tmp;
                nearest = i;
            }
        }
        //posList.Add(x);
        return nearest;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null && collide == true && GameObject.FindGameObjectsWithTag("Attacker").Length > 0)
        {
            var tmp = GameObject.FindGameObjectsWithTag("Attacker");
            foreach (GameObject foundOnce in tmp)
            {
                players.Add(foundOnce.transform);
            }
            collide = false;
            isMove = true;
        }
        else if (transform.parent != null && collide == true)
        {
            players.Clear();
        }
        else if (collide == true && GameObject.Find("Plane").GetComponent<RoundManager>().numPlayer <= 1 && isLose == false && isGoal == false )
        {
            isLose = true;
            transform.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (isMove && players.Count != 0)
        {
            MoveBall(players[FindPositionWithNearestObj()]);
        }
    }
}
