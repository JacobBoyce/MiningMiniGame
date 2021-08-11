using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject mineButton;
    public static GameController current;
    public event Action mineRockUIEnter, mineRockUIExit;
    //public PlayerController player;
    // Start is called before the first frame update

    public void Awake()
    {
        current = this;
    }
    void Start()
    {
        mineRockUIEnter += ShowMineUI;
        mineRockUIExit += HideMineUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMineUI()
    {
        mineButton.SetActive(true);
    }
    public void HideMineUI()
    {
        mineButton.SetActive(false);
    }

    public void MineRockUIEnter()
    {
        if(mineRockUIEnter != null)
        {
            mineRockUIEnter();
        }
    }

    public void MineRockUIExit()
    {
        if(mineRockUIExit != null)
        {
            mineRockUIExit();
        }
    }

    public void StartMining()
    {
        // choose what rock to mine \\ 
        // send and save for other scene \\
        PlayerPrefs.SetString("Ore", "Copper");

        //player.SaveStatsForMining();
        SceneManager.LoadScene("MiningScene");
    }
}