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
    private float batteryLevelMax;

    [Space(5)]
    [Header("Level Up Objects")]
    public GameObject lightObj;
    public GameObject scannerRangeObj;

    [Space(5)]
    public LevelPassSO saveObj;

    [Space(10)]
    [Header("Upgrade Values")]
    [SerializeField]
    public upgradeValues[] upVal;
    
    [Space(10)]
    public Image hPbarUI;
    private ThirdPersonMovement movementScript;
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
            hPbarUI.fillAmount = batteryLevel / batteryLevelMax;
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

    public void LoadSaveObjStats()
    {
        foreach(SaveStuffObj so in saveObj.saveList)
        {
            if(so.upName.Equals("Mining Drill"))
            {
                power = (int)upVal[0].upValue[so.level]; 
                //Debug.Log(upVal[0].upValue[so.level]);
            }
            else if(so.upName.Equals("Mining Focus"))
            {
                focus = (int)upVal[1].upValue[so.level];
                //Debug.Log(upVal[1].upValue[so.level]);
            }
            else if(so.upName.Equals("Battery Capacity"))
            {
                batteryLevel = (int)upVal[2].upValue[so.level];
                batteryLevelMax = batteryLevel;
                //Debug.Log(upVal[2].upValue[so.level]);
            }
            else if(so.upName.Equals("Battery Efficiency"))
            {
                drainRate = upVal[3].upValue[so.level];
                //Debug.Log(upVal[3].upValue[so.level]);
            }
            else if(so.upName.Equals("Scanner Range"))
            {
                scannerRangeObj.GetComponent<SphereCollider>().radius = upVal[4].upValue[so.level];
                //Debug.Log(upVal[4].upValue[so.level]);
            }
            else if(so.upName.Equals("Engine Power"))
            {
                movementScript.speed = upVal[5].upValue[so.level];
                //Debug.Log(upVal[5].upValue[so.level]);
            }
            else if(so.upName.Equals("Flash Light"))
            {
                lightObj.GetComponent<Light>().range = upVal[6].upValue[so.level];
                //Debug.Log(upVal[6].upValue[so.level]);
            }
        }
    }    
}

[System.Serializable]
public class upgradeValues
    {
        public string upName;
        public float[] upValue;
    }