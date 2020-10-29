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

    public GameObject btn;
    public Text stringUI;
    public float time = 3f;

    static int r_score = 0;
    static int b_score = 0;
    static int round = 0;

    int scoreTmp1 = 0;
    int scoreTmp2 = 0;
   
    bool endRound = false;
    bool endGame = false;
    private float timeRemaining = 3;

    // Start is called before the first frame update
    void Start()
    {
        round++;
        ball = GameObject.Find("Ball");
    }


    void IsWin(int z)
    {
        z++;
        ball.GetComponent<BallSolve>().isGoal = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (ball.GetComponent<BallSolve>().isLose == true)
        {
            if(GameObject.Find("UI In Game").GetComponent<UIManager>().namePlayer[0].text == "Player (Attacker)")
            {
                ball.GetComponent<BallSolve>().isLose = false;
                r_score++;

                BlueScore.text = b_score.ToString();
                RedScore.text = r_score.ToString();

                SetTitleUI();
                UIScore.SetActive(true);
                endRound = true;
            }
            else if(GameObject.Find("UI In Game").GetComponent<UIManager>().namePlayer[0].text == "Player (Defender)")
            {
                ball.GetComponent<BallSolve>().isLose = false;
                b_score++;

                BlueScore.text = b_score.ToString();
                RedScore.text = r_score.ToString();

                SetTitleUI();
                UIScore.SetActive(true);
                endRound = true;
            }
        }
        else if (ball.GetComponent<BallSolve>().isGoal == true)
        {
            if (GameObject.Find("UI In Game").GetComponent<UIManager>().namePlayer[0].text == "Player (Attacker)")
            {
                IsWin(b_score);
            }
            else if (GameObject.Find("UI In Game").GetComponent<UIManager>().namePlayer[0].text == "Player (Defender)")
            {
                IsWin(r_score);
            }
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

            SetTitleUI();
            UIScore.SetActive(true);
            endRound = true;
        }
        if(endGame == false && endRound == true)
        {
            if (UIScore.activeInHierarchy)
            {
                StartPause();
            }
        }
        else if (endGame && endRound == false)
        {
            StartCoroutine(PauseGame(120));
        }
        
        if (round == GameObject.Find("GameManager").GetComponent<GameManager>().matchTmp)
        {
            endGame = true;
        }
    }
    void SetTitleUI()
    {
        if (round < GameObject.Find("GameManager").GetComponent<GameManager>().matchTmp)
        {
            stringUI.text = "MATCH " + round;
        }
        else
        {
            endGame = true;
            btn.gameObject.SetActive(true);
            stringUI.text = "END GAME ";
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
        
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
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
   /* void delete()
    {
        for (int i = 0; i < obj.Count; i++)
        {
            Destroy(obj[i]);
        }
    }*/

    void AttackerWin()
    {
        SetTitleUI();

        BlueScore.text = b_score.ToString();
        RedScore.text = r_score.ToString();
        UIScore.SetActive(true);
        endRound = true;
    }
    
}
