using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int match;
    public int timeLimit;
    public int energyBar;
    private Vector3 t_position;
    
    public Vector3 t_Position
    {
        get { return t_position; }
        set
        {
            t_position = value;
        }
    }
    void Aware()
    {
        
        MakeSingleton();
    }


    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
