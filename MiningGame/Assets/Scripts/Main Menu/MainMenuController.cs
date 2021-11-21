using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;
public class MainMenuController : MonoBehaviour
{
    //save money amount, and what level of gear you have
    public ShopController shopController;
    public TextMeshProUGUI miningText, moneyText;
    private MainProgressSave saveProgress;
    private SaveGearUpgrade tempUp;

    public int money;
    public void Awake()
    {
        saveProgress = new MainProgressSave();
        //load save file
        if(File.Exists(Application.persistentDataPath  + "MainSaveProgress" + ".sav"))
        {
            saveProgress = SaveManager.Load<MainProgressSave>("MainSaveProgress");
            //set all values of save file
            money = saveProgress.money;
            moneyText.text = "Money: $" + money;

            foreach(SaveGearUpgrade gu in saveProgress.gearUpgrades)
            {
                for(int i = 0; i < shopController.gearInfo.Count; i++)
                {
                    if(gu.upName.Equals(shopController.gearInfo[i].upName))
                    {
                        shopController.gearInfo[i].Setlevel(gu.level);
                    }
                }
            }
        }
        else
        {
            //set base value of save file stuff
            //money = 0;
            saveProgress.gearUpgrades = new List<SaveGearUpgrade>();
            InitGearSaveFile();
        }
        

        if(PlayerPrefs.HasKey("MoneyCalculated") && PlayerPrefs.GetInt("MoneyCalculated") == 1)
        {
            Debug.Log("Money added, and file deleted");

            miningText.text = "Start Mining";

            money += PlayerPrefs.GetInt("MoneyMade");
            moneyText.text = "Money: $" + money;
            saveProgress.money = money;
            SaveManager.Save<MainProgressSave>(saveProgress, "MainSaveProgress");

            //delete save file for mining search
            SaveManager.DeleteFile("MiningInstProgress");

            //reset value to say money was added and if the try to cheat itll be safe
            PlayerPrefs.SetInt("MoneyCalculated", 0);
        }
        else
        {
            if(File.Exists(Application.persistentDataPath  + "MiningInstProgress" + ".sav"))
            {
                Debug.Log("File detected");
                miningText.text = "Resume";
            }
            else
            {
                Debug.Log("new adventure ready");
                miningText.text = "Start Mining";
            }
        }
    }

    public void StartMiningSearch()
    {
        SceneManager.LoadScene("MiningSearch");
    }

    public void InitGearSaveFile()
    {
        tempUp = new SaveGearUpgrade();
        tempUp.upName = shopController.gearInfo[0].upName;
        tempUp.level = shopController.gearInfo[0].level;
        saveProgress.gearUpgrades.Add(tempUp);

        tempUp = new SaveGearUpgrade();
        tempUp.upName = shopController.gearInfo[1].upName;
        tempUp.level = shopController.gearInfo[1].level;
        tempUp.level = 0;
        saveProgress.gearUpgrades.Add(tempUp);

        tempUp = new SaveGearUpgrade();
        tempUp.upName = shopController.gearInfo[2].upName;
        tempUp.level = shopController.gearInfo[2].level;
        tempUp.level = 0;
        saveProgress.gearUpgrades.Add(tempUp);
        
        tempUp = new SaveGearUpgrade();
        tempUp.upName = shopController.gearInfo[3].upName;
        tempUp.level = shopController.gearInfo[3].level;
        tempUp.level = 0;
        saveProgress.gearUpgrades.Add(tempUp);

        tempUp = new SaveGearUpgrade();
        tempUp.upName = shopController.gearInfo[4].upName;
        tempUp.level = shopController.gearInfo[4].level;
        tempUp.level = 0;
        saveProgress.gearUpgrades.Add(tempUp);
    }

    public void SaveGameProgress()
    {
        foreach(SaveGearUpgrade spGU in saveProgress.gearUpgrades)
        {
            MasterGearInfo mg2 = shopController.gearInfo.Find((x) => x.upName.Equals(spGU.upName));
            spGU.level = mg2.level;
        }
        saveProgress.money = money;

        SaveManager.Save<MainProgressSave>(saveProgress, "MainSaveProgress");
    }
}
