using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

[System.Serializable]
public class AttackerHandle : CharacterManager
{
    public Material[] t_materials; 
    public float carryingSpeed = 0.75f;
    GameObject t_ball;
    bool t_isOwnBall = false;
    bool t_detectBall = false;
    Vector3 ballPosition;
    Vector3 lookAtBall;
    Quaternion Rotation;
    float timeLimitTmp = 0f;
    [HideInInspector]
    public bool isCollided = false;
    [HideInInspector]
    public int scoreBlue = 0;

    private void Start()
    {
        t_ball = GameObject.Find("Ball");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball" && t_detectBall == true)
        {
            //t_moving = true;
            t_movingNormal = false;
            t_isOwnBall = true;
            t_detectBall = false;
        }
        if (other.gameObject.tag == "Gate")
        {
            if (transform.tag == "CarryingAttacker")
            {
                Destroy(this.gameObject);
            }
            if (transform.tag == "Attacker")
            {
                Destroy(this.gameObject);
            }
        }
        // being destroy when reaching the opponent's fence at Normal speed 
        if (other.gameObject.tag == "Fence")
        {
            if (gameObject.tag == "CarryingAttacker")
            {
                t_ball.transform.SetParent(null);

                transform.GetComponent<Renderer>().material.SetFloat("_Metallic", 0.4f);
            }
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Defender" && other.gameObject.layer == 9)
        {
            t_ball.transform.SetParent(null);
            ChangeTagAndLayer("Cooldown", 8);
            transform.GetComponent<MeshRenderer>().material = t_materials[0];

            transform.GetComponent<Renderer>().material.SetFloat("_Metallic", 0.4f);

            t_detecter.SetActive(false);
            isCollided = true;
            t_isOwnBall = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ball" && t_detectBall == true)
        {
            t_movingNormal = false;
            t_isOwnBall = true;
        }
    }

    void Move(Vector3 v, float s)
    {
        lookAtBall =  new Vector3(v.x - transform.position.x, 0, v.z - transform.position.z);

        if (lookAtBall != new Vector3(0, 0, 0))
        {
            Rotation = Quaternion.LookRotation(lookAtBall);
        }
        t_detecter.SetActive(true);
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y,
            transform.position.z), new Vector3(v.x, transform.position.y, v.z), s * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, 20f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(t_ball != null)
        {
            if (t_ball.GetComponent<BallSolve>().isGoal == true)
            {
                scoreBlue++;
            }
        }
    }

    void MoveStraight(float s)
    {
        t_detecter.SetActive(true);
        if (GameObject.Find("Plane").GetComponent<RoundManager>().numOfTeam == 1)
        {
            transform.position += Vector3.forward * s * Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else if (GameObject.Find("Plane").GetComponent<RoundManager>().numOfTeam == 2)
        {
            transform.position -= Vector3.forward * s * Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    void ChangeTagAndLayer(string st, int i)
    {
        transform.gameObject.tag = st;
        transform.gameObject.layer = i;
    }

    private void FixedUpdate()
    {
        // all attacker team move forward pos ball
        if (GameObject.Find("GameManager").GetComponent<GamePlay>().hasCarryingPlayer == false && !isCollided)
        {
            Move(t_ball.transform.position, t_normalSpeed);
            t_movingNormal = true;
            t_detectBall = true;
        }
        if(isCollided == false)
        {
            if (t_isOwnBall == true)
            {
                //change tag of parent carry ball
                ChangeTagAndLayer("CarryingAttacker", 10);
                //set child of carrying attacker when attacker move the ball move follows 
                t_ball.transform.SetParent(transform.GetChild(1));
                transform.GetComponent<Renderer>().material.SetFloat("_Metallic", 1f);
                t_ball.GetComponent<BallSolve>().collide = true;
                t_ball.transform.localPosition = new Vector3(0, 0, 0);
                
                MoveStraight(carryingSpeed);
            }
            // the others moved straight
            else if (t_isOwnBall == false && GameObject.Find("GameManager").GetComponent<GamePlay>().hasCarryingPlayer == true)
            {
                MoveStraight(t_normalSpeed);
            }
        }
        else if(isCollided)
        {
            //Cooldown time reactivate
            timeLimitTmp += Time.deltaTime;
            if (timeLimitTmp >= t_reactivateTime)
            {
                ChangeTagAndLayer("Attacker", 8);
                timeLimitTmp = 0f;
                transform.GetComponent<MeshRenderer>().material = t_materials[1];
                isCollided = false;
                t_isOwnBall = false;
            }
        }
    }
}


//error: 
//1. this is no detect then defender standing
//2. ball move so fast
//3. material error

//addition
//ball collide goal to win -> score blue
//attacker collide fence then ball move to nearest attacker
//end attacker callide the defender to lose -> score red
//hight light energy
