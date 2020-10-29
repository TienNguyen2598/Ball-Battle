using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickPlayButton()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
