using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject u_bgR;
    public GameObject u_cellR;

    public GameObject u_bgB;
    public GameObject u_cellB;

    public Image[] energy;
    public Image[] highlight;
    [HideInInspector]
    public Text u_time;
    int maxPointEnergy;
    float timeTmp = 0;
    private void Start()
    {
        maxPointEnergy = GameObject.Find("GameManager").GetComponent<GameManager>().energyBar;
        timeTmp = GameObject.Find("GameManager").GetComponent<GameManager>().timeLimit;
        u_time.text = timeTmp.ToString();
        for(int i = 0; i< maxPointEnergy; i += 1)
        {
            GameObject go = Instantiate(u_cellR);
            go.transform.SetParent(u_bgR.transform);
        }
        for (int i = 0; i < maxPointEnergy; i += 1)
        {
            GameObject go = Instantiate(u_cellB);
            go.transform.SetParent(u_bgB.transform);
        }
    }
    void Update()
    {
        timeTmp -= Time.deltaTime;
        int tmp = (int)timeTmp;
        u_time.text = tmp.ToString();
        if(GameObject.Find("Plane") != null)
        {
            if (GameObject.Find("Plane").GetComponent<RoundManager>().t_wall.activeInHierarchy)
            {
                energy[0].fillAmount += Time.deltaTime / maxPointEnergy;
                energy[1].fillAmount += Time.deltaTime / maxPointEnergy;
            }
        }
        
        for(int i = 1; i <= maxPointEnergy; i++)
        {
            if (energy[0].fillAmount >= (float)i / (float)maxPointEnergy)
            {
                highlight[0].fillAmount = (float)i / (float)maxPointEnergy;
            }
            else if(energy[0].fillAmount >= 0.99f)
            {
                highlight[0].fillAmount = 1;
            }

            if (energy[1].fillAmount >= (float)i / (float)maxPointEnergy)
            {
                highlight[1].fillAmount = (float)i / (float)maxPointEnergy;
            }
            else if (energy[1].fillAmount >= 0.99f)
            {
                highlight[1].fillAmount = 1;
            }
        }
    }
}
