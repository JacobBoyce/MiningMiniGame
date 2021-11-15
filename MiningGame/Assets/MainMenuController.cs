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
    public TextMeshProUGUI miningText, moneyText;
    private MainProgressSave saveProgress;

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
        }
        else
        {
            //set base value of save file stuff
            money = 0;
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
}
