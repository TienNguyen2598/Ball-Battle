using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class GamePlay : MonoBehaviour
{
    public GameObject[] t_barV;
    public GameObject t_ball;
    Vector3 gm;
    [HideInInspector]
    public bool t_StartRound = true;
    [HideInInspector]
    public bool hasCarryingPlayer = false;
    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>().t_Position;
    }
    private void Update()
    {
        if (t_StartRound == true)
        {
            gm = new Vector3(Random.Range(t_barV[0].transform.position.x, t_barV[1].transform.position.x), t_ball.transform.position.y,
        Random.Range(t_barV[0].transform.position.z, t_barV[1].transform.position.z));
            t_ball.SetActive(true);
            t_ball.transform.position = gm;
            t_StartRound = false;
        }
        if (t_ball.transform.parent != null)
        {
            hasCarryingPlayer = true;
        }
        else { hasCarryingPlayer = false; }
    }
}
