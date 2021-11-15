using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using System.IO;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class GameController : MonoBehaviour
{
    public InventoryBagUI invUI;
    public GameObject endGameUI;
    public GameObject mineButton;
    private GameObject playerObj;
    public static GameController current;
    public event Action mineRockUIEnter, mineRockUIExit;
    public bool outOfEnergy;

    //public List<inventoryItems> inventory = new List<inventoryItems>();
    [SerializeField] public InventoryBag bag = new InventoryBag();
    [Serializable]
    public struct OrePrices
    {
        public string oreName;
        public int orePriceL;
        public int orePriceS;
    }
    [SerializeField] public OrePrices[] orePrices; 
    private int calcAmount;
    private SaveMiningProgressObj savebag;

    [SerializeField] public List<SpawningPoint> _spawnPoints;

    private int moneyMade;

    //public PlayerController player;
    // Start is called before the first frame update

    public void Awake()
    {
        current = this;
        outOfEnergy = false;
        playerObj = GameObject.Find("Player");
        savebag = new SaveMiningProgressObj();
        if(File.Exists(Application.persistentDataPath  + "MiningInstProgress" + ".sav"))
        {
            savebag = SaveManager.Load<SaveMiningProgressObj>("MiningInstProgress");
            bag = savebag.bag;
            playerObj.transform.position = savebag.playerPos;
            //this fixes the loading of the players position. (error lies in the update function of movement)
            playerObj.GetComponent<ThirdPersonMovement>().StartCoroutine("TurnOnMove");
            playerObj.GetComponent<PlayerController>().batteryLevel = savebag.batteryLevel;
        }
        else
        {
            playerObj.GetComponent<ThirdPersonMovement>().StartCoroutine("TurnOnMove");
        }
    }
    void Start()
    {
        mineRockUIEnter += ShowMineUI;
        mineRockUIExit += HideMineUI;

        //check player pref bool if player just got done mining.
            //if true then reload player position from player pref and update inventory
        if(PlayerPrefs.HasKey("DoneMining") && PlayerPrefs.GetString("DoneMining").Equals("true"))
        {                                                   
            PlayerPrefs.SetString("DoneMining","false");

            inventoryItems item = new inventoryItems();
            //save values into the inventory then save the inventory
            for(int i = 0; i < bag.inv.Length; i++)
            {
                if(bag.inv[i].rockType == GetRockType(PlayerPrefs.GetString("Ore")))
                {
                    bag.inv[i].amountL += PlayerPrefs.GetInt("LargeOreTotal");
                    bag.inv[i].amountS += PlayerPrefs.GetInt("SmallOreTotal");
                }
            }

            SaveGameState();
            //savebag.bag = bag;
            //savebag.batteryLevel = playerObj.GetComponent<PlayerController>().batteryLevel;
            //SaveManager.Save<SaveMiningProgressObj>(savebag, "MiningInstProgress");
        }

        invUI.PopulateInvUI(bag);

        SpawnRockPoints();
    }

    public void Update()
    {
        if(outOfEnergy)
        {
            outOfEnergy = false;
            //end the game and stuff/ maybe set time scale to 0 or pop up UI
            //animate all ui to move off screen and blur background then bring in endgame UI
            GameObject[] uiEle = GameObject.FindGameObjectsWithTag("MineSearchUI");
            for(int i = 0; i < uiEle.Length; i++)
            {
                uiEle[i].SetActive(false);
            }
            endGameUI.SetActive(true);
            CalculateMoney();
            PlayerPrefs.SetInt("MoneyMade", moneyMade);
            //in main menu check if this player pref is 1, 
                //if it is and the file exsists then add the money and delete the file
            PlayerPrefs.SetInt("MoneyCalculated", 1);

            SaveGameState();
            //when back at the main menu
                //-delete the file after the money has been transfered (thru player prefs, then set it back to 0)
                //-check player prefs if money has been add to stash?
        }
    }

    public void CalculateMoney()
    {
        //cycle thru inv and get amount and then multiply it by rock amount
        for(int i = 0; i < bag.inv.Length; i ++)
        {
            if(bag.inv[i].rockType == inventoryItems.RockType.COPPER)
            {
                //use this var if needed to use in the UI or something
                int temp = GetOrePrice(bag.inv[i].rockName, bag.inv[i].amountL, bag.inv[i].amountS);
                calcAmount += temp;
            }
            else if(bag.inv[i].rockType == inventoryItems.RockType.IRON)
            {
                int temp = GetOrePrice(bag.inv[i].rockName, bag.inv[i].amountL, bag.inv[i].amountS);
                calcAmount += temp;
            }
            else if(bag.inv[i].rockType == inventoryItems.RockType.COAL)
            {
                int temp = GetOrePrice(bag.inv[i].rockName, bag.inv[i].amountL, bag.inv[i].amountS);
                calcAmount += temp;
            }
        }
        moneyMade = calcAmount;
    }

    public int GetOrePrice(string rName, int sizeL, int sizeS)
    {
        for(int i = 0; i < orePrices.Length; i++)
        {
            if(orePrices[i].oreName.Equals(rName))
            {
                return (orePrices[i].orePriceL * sizeL) + (orePrices[i].orePriceS * sizeS);
            }
        }
        return 0;
    }

    void OnApplicationPause()
    {
        SaveGameState();
    }

    void OnApplicationQuit()
    {
        SaveGameState();
    }

    public void SaveGameState()
    {
        savebag.bag = bag;
        savebag.batteryLevel = playerObj.GetComponent<PlayerController>().batteryLevel;
        savebag.playerPos = playerObj.transform.position;
        SaveManager.Save<SaveMiningProgressObj>(savebag, "MiningInstProgress");
    }

    public void SpawnRockPoints()
    {
        int spawnMax = 2, spawnCount = 0;

        foreach(SpawningPoint sp in _spawnPoints)
        {
            sp.TurnOff();
        }

        while(spawnCount < spawnMax)
        {
            int ind1 = UnityEngine.Random.Range(0, _spawnPoints.Count-1);

            if(_spawnPoints[ind1].isOff == false)
            {
                break;
            }
            else
            {
                _spawnPoints[ind1].TurnOn();
                _spawnPoints[ind1].MoveRockSpot();
                spawnCount++;
            }
        }
    }

    public void DeleteFilez()
    {
        SaveManager.DeleteFile("MiningInstProgress");
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
        PlayerPrefs.SetString("Ore", "Copper");
        
        playerObj.GetComponent<PlayerController>().SpendMiningEnergy();
        SaveGameState();

        SceneManager.LoadScene("MiningScene");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public inventoryItems.RockType GetRockType(string rockname)
    {
        foreach(inventoryItems.RockType rockt in Enum.GetValues(typeof(inventoryItems.RockType)))
        {
            if(rockname.ToLower().Equals(rockt.ToString().ToLower()))
            {
                return rockt;
            }
        }
        return inventoryItems.RockType.NONE;
    }
}

[System.Serializable]
public class InventoryBag
{
    public inventoryItems[] inv = new inventoryItems[3];
    //public List<inventoryItems> inv2 = new List<inventoryItems>();
}

[System.Serializable]
public class inventoryItems
{
    [Serializable]
    public enum RockType
    {
        COPPER,
        IRON,
        COAL,
        NONE
    }
    [OdinSerialize] public string rockName;
    [OdinSerialize] public RockType rockType;
    [OdinSerialize] public int amountL;
    [OdinSerialize] public int amountS;
}