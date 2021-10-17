using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using System.IO;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class GameController : MonoBehaviour
{
    public InventoryBagUI invUI;
    public GameObject mineButton;
    private GameObject playerObj;
    public static GameController current;
    public event Action mineRockUIEnter, mineRockUIExit;

    //public List<inventoryItems> inventory = new List<inventoryItems>();
    [SerializeField] public InventoryBag bag = new InventoryBag();

    //public PlayerController player;
    // Start is called before the first frame update

    public void Awake()
    {
        current = this;
        playerObj = GameObject.Find("Player");
    }
    void Start()
    {
        mineRockUIEnter += ShowMineUI;
        mineRockUIExit += HideMineUI;

        #region Save Data in List form (doesnt work)
        /*if(File.Exists(Application.persistentDataPath  + "Inventory" + ".sav"))
        {
            Debug.Log("file exsists");
            bag = SaveManager.Load<InventoryBag>("Inventory");
        }

        //check player pref bool if player just got done mining.
            //if true then reload player position from player pref and update inventory
        if(PlayerPrefs.HasKey("DoneMining") && PlayerPrefs.GetString("DoneMining").Equals("true"))
        {
            playerObj.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"),
                                                    PlayerPrefs.GetFloat("PlayerPosY"),
                                                    PlayerPrefs.GetFloat("PlayerPosZ"));
            PlayerPrefs.SetString("DoneMining","false");

            //save values into the inventory then save the inventory
            inventoryItems item = bag.inv2.Find(x => x.rockType == GetRockType(PlayerPrefs.GetString("Ore")));
            Debug.Log(item.rockType);
            int index = bag.inv2.FindIndex(x => x.rockType == GetRockType(PlayerPrefs.GetString("Ore")));
            Debug.Log("Index " + index);
            item.amountL += PlayerPrefs.GetInt("LargeOreTotal");
            item.amountS += PlayerPrefs.GetInt("SmallOreTotal");
            bag.inv2[index] = item;
            SaveManager.Save<InventoryBag>(bag, "Inventory");

        }*/
        #endregion
    
        if(File.Exists(Application.persistentDataPath  + "Inventory" + ".sav"))
        {
            Debug.Log("file exsists");
            bag = SaveManager.Load<InventoryBag>("Inventory");
        }

        //check player pref bool if player just got done mining.
            //if true then reload player position from player pref and update inventory
        if(PlayerPrefs.HasKey("DoneMining") && PlayerPrefs.GetString("DoneMining").Equals("true"))
        {
            playerObj.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"),
                                                    PlayerPrefs.GetFloat("PlayerPosY"),
                                                    PlayerPrefs.GetFloat("PlayerPosZ"));
            PlayerPrefs.SetString("DoneMining","false");


            inventoryItems item = new inventoryItems();
            //save values into the inventory then save the inventory
            for(int i = 0; i < bag.inv.Length; i++)
            {
                if(bag.inv[i].rockType == GetRockType(PlayerPrefs.GetString("Ore")))
                {
                    Debug.Log(item.rockType);
                    bag.inv[i].amountL += PlayerPrefs.GetInt("LargeOreTotal");
                    bag.inv[i].amountS += PlayerPrefs.GetInt("SmallOreTotal");
                }
            }
            SaveManager.Save<InventoryBag>(bag, "Inventory");
        }

        invUI.PopulateInvUI(bag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteFilez()
    {
        SaveManager.DeleteFile("Inventory");
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
        //save player position for when they come back
        PlayerPrefs.SetFloat("PlayerPosX",playerObj.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY",playerObj.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ",playerObj.transform.position.z);

        //player.SaveStatsForMining();
        SceneManager.LoadScene("MiningScene");
    }

    /*public void PrintInventory()
    {
        foreach(inventoryItems rock in inventory)
        {
            Debug.Log(rock.rockType + " " + rock.amountL + " : " + rock.amountS);
        }
    }*/

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