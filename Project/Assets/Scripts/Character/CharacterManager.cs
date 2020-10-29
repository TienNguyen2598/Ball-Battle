using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public float t_energyRegeneration = 0.5f;
    public int t_energyCost = 2;
    public float t_spawnTime = 0.5f;
    public float t_reactivateTime = 2.5f;
    public float t_normalSpeed = 1.5f;
    public GameObject t_detecter;
    protected bool t_movingNormal = false;
    protected bool t_moving = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
