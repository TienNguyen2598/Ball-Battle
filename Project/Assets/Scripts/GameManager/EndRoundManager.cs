using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[System.Serializable]
public class EndRoundManager : MonoBehaviour
{
    GameObject ball;
    public Text RedScore;
    public Text BlueScore;
    public GameObject UIScore;

    public List<GameObject> obj;
    static int r_score = 0;
    static int b_score = 0;
    public float time = 3f;
    float i = 0f;
    static int round = 0;
    bool endRound = false;
    bool endGame = false;
    private float timeRemaining = 3;

    // Start is called before the first frame update
    void Start()
    {
        round++;
        ball = GameObject.Find("Ball");
    }
    // Update is called once per frame
    void Update()
    {
        if (ball.GetComponent<BallSolve>().isLose == true)
        {
            ball.GetComponent<BallSolve>().isLose = false;
            r_score++;
            RedScore.text = r_score.ToString();
            UIScore.SetActive(true);
            endRound = true;
        }
        else if (ball.GetComponent<BallSolve>().isGoal == true)
        {
            b_score++;
            StartCoroutine(WaitingTime(3f));
            //AttackerWin();
        }
        else if (GameObject.Find("UI In Game").GetComponent<UIManager>().u_time.text == "0" && endRound == false)
        {
            ball.GetComponent<BallSolve>().isLose = false;
            r_score++;
            RedScore.text = r_score.ToString();

            ball.GetComponent<BallSolve>().isGoal = false;
            b_score++;
            BlueScore.text = b_score.ToString();


            UIScore.SetActive(true);
            endRound = true;

            //delete();
        }
        if(endGame == false && endRound == true)
        {
            if (UIScore.activeInHierarchy)
            {
                StartPause();
            }
        }
        else
        {

        }
        
        if (round == GameObject.Find("GameManager").GetComponent<GameManager>().match)
        {
            endGame = true;
        }
    }
    public void StartPause()
    {
        // how many seconds to pause the game
        StartCoroutine(PauseGame(timeRemaining));
    }
    IEnumerator PauseGame(float s)
    {
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + s;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;
        EndMatch();
    }

    IEnumerator WaitingTime(float s)
    {
        float pauseEndTime = Time.realtimeSinceStartup + s;

        ball.GetComponent<BallSolve>().boom.gameObject.SetActive(true);
        ball.GetComponent<BallSolve>().isGoal = false;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        //EndMatch();
        AttackerWin();
    }

    void EndMatch()
    {
        UIScore.SetActive(false);
        deleteObjects("Cooldown");
        if (endGame == false)
        {
            SceneManager.LoadScene("Gameplay");
        }

    }
    void deleteObjects(string s)
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag(s))
        {
            Destroy(o);
        }
    }
    void delete()
    {
        for (int i = 0; i < obj.Count; i++)
        {
            Destroy(obj[i]);
        }
    }

    void AttackerWin()
    {
        BlueScore.text = b_score.ToString();
        UIScore.SetActive(true);
        endRound = true;
    }
    
}
