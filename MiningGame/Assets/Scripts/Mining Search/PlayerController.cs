using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Level up values")]
    public int power;
    public int focus;
    public float batteryLevel, drainRate, miningCost;
    
    [Space(10)]
    public Image hPbarUI;
    private ThirdPersonMovement movementScript;
    private bool msgSent;

    public void Awake()
    {
        movementScript = this.GetComponent<ThirdPersonMovement>();
        hPbarUI.fillAmount = batteryLevel / 100;
        msgSent = false;
        //get values from player prefsand set them here
        //playerprefs.get "mining power" 60
        //playerprefs.get "mining power level" 1
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

    public void SaveStatsForMining()
    {
        PlayerPrefs.SetInt("Ppower", power);
        PlayerPrefs.SetInt("Pfocus", focus);
    }
}
