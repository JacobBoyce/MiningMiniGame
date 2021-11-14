using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int level;
    public int power, focus, curXP, maxXP;
    public float batteryLevel, drainRate, miningCost;
    private ThirdPersonMovement movementScript;
    public Image hPbarUI;
    private bool msgSent;

    public void Awake()
    {
        movementScript = this.GetComponent<ThirdPersonMovement>();
        hPbarUI.fillAmount = batteryLevel / 100;
        msgSent = false;
    }

    public void Update()
    {
        if(batteryLevel > 0)
        {
            DrainBattery();
        }
        else if(batteryLevel <= 0 && msgSent == false)
        {
            movementScript.canMove = false;
            //send message to gamecontroller
            GameController.current.outOfEnergy = true;
            msgSent = true;
        }
    }

    public void DrainBattery()
    {
        if(movementScript.isMoving)
        {
            batteryLevel -= drainRate; 
            hPbarUI.fillAmount = batteryLevel / 100;
        }
    }

    public void SpendMiningEnergy()
    {
        batteryLevel -= miningCost;
    }

    public void CheckLevelUp()
    {
        if(curXP >= maxXP)
        {
            level++;
            curXP -= maxXP;
            UpdateMaxXp();
            if(curXP <= 0) 
            {
                curXP = 0;
            }
            else
            {
                CheckLevelUp();
            }
        }
    }

    public void SaveStatsForMining()
    {
        PlayerPrefs.SetInt("Plevel", level);
        PlayerPrefs.SetInt("Ppower", power);
        PlayerPrefs.SetInt("Pfocus", focus);
    }

    public void UpdateMaxXp()
    {
        maxXP += maxXP * level;
    }
}
