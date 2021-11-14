using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveObjMining
{
    public InventoryBag bag;
    public float batteryLevel;

    public SaveObjMining()
    {
        bag = new InventoryBag();
        batteryLevel = 100;
    }
}

[Serializable]
public class SaveMiningProgressObj
{
    //public SaveObjMining savObj;
    //inventory
    //battery level
    //other things if needed
    //public event Action loadedFile, saveFile;
    [SerializeField]
    public InventoryBag bag;
    public float batteryLevel;
    public Vector3 playerPos;


    void Awake()
    {
        //savObj = new SaveObjMining();
        //saveFile += SaveMiningFile;
        //load info to objects then call loaded file for the affected object to save the info locally
        //StartCoroutine(LoadStuff());
    }

    /*public IEnumerator LoadStuff()
    {
        //load
        savObj = SaveManager.Load<SaveObjMining>("MiningInstProgress");
        //Debug.Log("BattLevel: " + savObj.batteryLevel);
        yield return new WaitForSeconds(.5f);
        LoadedFile();
    }

    public void SaveMiningFile()
    {
        SaveManager.Save<SaveObjMining>(savObj, "MiningInstProgress");
    }

    public void SaveFile()
    {
        if(saveFile != null)
        {
            saveFile();
        }
    }

    public void LoadedFile()
    {
        if(loadedFile != null)
        {
            loadedFile();
        }
    }*/
}
