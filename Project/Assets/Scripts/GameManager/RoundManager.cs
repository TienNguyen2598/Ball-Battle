using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public SetupLandOfTeams[] t_teams;
    public GameObject[] t_pointArea;
    public GameObject[] t_pointSpawnOfGateAndFence;
    public GameObject t_wall; 

    public GameObject t_bluePlayer;
    public GameObject t_redPlayer;
    public int numOfTeam = 0;
    [HideInInspector]
    static int t_checkTeamAttack = 0;//1 - attacker, 2 - defender
    [HideInInspector]
    public Vector3 point;
    string t_type = "";

    string preObject = "";
    float energyPercent = 0f;
    float timeTmp = 0f;
    bool isCooldownTime = false;
    private void Start()
    {
        //t_checkTeamAttack = Random.Range(1, 3);
        if(t_checkTeamAttack == 0)
        {
            t_checkTeamAttack = 1;
            return;
        }
        else if(t_checkTeamAttack == 1)
        {
            t_checkTeamAttack = 2;
            return;
        }
        else if(t_checkTeamAttack == 2)
        {
            t_checkTeamAttack = 1;
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        numOfTeam = t_checkTeamAttack;
        // determine land field 
        if (t_checkTeamAttack == 1)
        {
            // set land field for two teams 
            t_teams[0].s_landField.transform.position = new Vector3(t_pointArea[0].transform.position.x, t_teams[0].s_landField.transform.position.y, t_pointArea[0].transform.position.z);
            t_teams[1].s_landField.transform.position = new Vector3(t_pointArea[1].transform.position.x, t_teams[1].s_landField.transform.position.y, t_pointArea[1].transform.position.z);

            // set the gates and fences for two teams
            t_teams[0].s_GateAndFence.transform.position = new Vector3( t_teams[0].s_GateAndFence.transform.position.x, 
                t_teams[0].s_GateAndFence.transform.position.y, t_pointSpawnOfGateAndFence[0].transform.position.z);
            t_teams[1].s_GateAndFence.transform.position = new Vector3( t_teams[1].s_GateAndFence.transform.position.x,
                t_teams[1].s_GateAndFence.transform.position.y, t_pointSpawnOfGateAndFence[1].transform.position.z);

            ShowEveryObject();
        }
        else if (t_checkTeamAttack == 2)
        {
            t_teams[0].s_landField.transform.position = new Vector3(t_pointArea[1].transform.position.x, t_teams[1].s_landField.transform.position.y, t_pointArea[1].transform.position.z);
            t_teams[1].s_landField.transform.position = new Vector3(t_pointArea[0].transform.position.x, t_teams[0].s_landField.transform.position.y, t_pointArea[0].transform.position.z);

            t_teams[0].s_GateAndFence.transform.position = new Vector3(t_teams[0].s_GateAndFence.transform.position.x,
                t_teams[0].s_GateAndFence.transform.position.y, t_pointSpawnOfGateAndFence[1].transform.position.z);
            t_teams[1].s_GateAndFence.transform.position = new Vector3(t_teams[1].s_GateAndFence.transform.position.x,
                t_teams[1].s_GateAndFence.transform.position.y, t_pointSpawnOfGateAndFence[0].transform.position.z);

            ShowEveryObject();
        }
        // click  left mouse to spawn player
        if(Input.GetMouseButtonDown(0))
        {
            GetPositionPlayer();
        }

        if(t_type != "" && isCooldownTime == false)
        {
            // instantiate player
            if (t_type == "Land Field B")
            {
                energyPercent = GameObject.Find("UI In Game").GetComponent<UIManager>().energy[1].fillAmount * GameObject.Find("GameManager").GetComponent<GameManager>().energyBar;
                if (energyPercent >= t_bluePlayer.GetComponent<AttackerHandle>().t_energyCost)
                {
                    
                    Instantiate(t_bluePlayer, new Vector3(point.x, t_bluePlayer.transform.position.y, point.z), Quaternion.identity);
                    preObject = t_type;// assignment name of type spawn;
                    timeTmp = 0;
                    GameObject.Find("UI In Game").GetComponent<UIManager>().energy[1].fillAmount -=
                        ((float)t_bluePlayer.GetComponent<AttackerHandle>().t_energyCost / ((float)GameObject.Find("GameManager").GetComponent<GameManager>().energyBar));
                }
            }
            else if (t_type == "Land Field R")
            {
                energyPercent = GameObject.Find("UI In Game").GetComponent<UIManager>().energy[0].fillAmount * GameObject.Find("GameManager").GetComponent<GameManager>().energyBar;
                if (energyPercent >= t_redPlayer.GetComponent<ManagerDefender>().t_energyCost)
                {
                    Debug.Log(energyPercent);
                    Instantiate(t_redPlayer, new Vector3(point.x, t_redPlayer.transform.position.y, point.z), Quaternion.identity);
                    preObject = t_type; // assignment name of type spawn;
                    timeTmp = 0;
                    GameObject.Find("UI In Game").GetComponent<UIManager>().energy[0].fillAmount -=
                        ((float)t_redPlayer.GetComponent<ManagerDefender>().t_energyCost / ((float)GameObject.Find("GameManager").GetComponent<GameManager>().energyBar));
                }
            }
            
            t_type = "";
        }
        if (preObject != "")
        {
            if (t_type == "Land Field B" && preObject == "Land Field B")
            {
                isCooldownTime = true;
                timeTmp += Time.deltaTime;
                
                if (timeTmp >= t_bluePlayer.GetComponent<AttackerHandle>().t_spawnTime)
                {
                    isCooldownTime = false;
                }
            }
            else if (t_type == "Land Field R" && preObject == "Land Field R" )
            {
                isCooldownTime = true;
                timeTmp += Time.deltaTime;
                if (timeTmp >= t_redPlayer.GetComponent<ManagerDefender>().t_spawnTime)
                {
                    isCooldownTime = false;
                }
            }
            else
            {
                isCooldownTime = false;
            }
        }
    }

    void GetPositionPlayer()
    {
        //get position of mouse's pointer 
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            point = hit.point;
            if ( hit.collider != null)
            {
                //get name of gameObject when is detect collider
                t_type = hit.transform.name;
            }
        }
    }

    void ShowEveryObject()
    {
        for(int i = 0; i< t_teams.Length; i++)
        {
            t_teams[i].s_GateAndFence.SetActive(true);
        }
        t_wall.SetActive(true);
    }
}

[System.Serializable]
public class SetupLandOfTeams
{
    public GameObject s_landField;
    public GameObject s_GateAndFence;
}
