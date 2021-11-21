using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MainProgressSave
{
    public int money;

    //level of gear for game stuff
    [SerializeField] public List<SaveGearUpgrade> gearUpgrades;
}
//some logic that lives somewhere about how these level up
//this is just for levels

[Serializable]
public class SaveGearUpgrade
{
    [SerializeField] public string upName;
    [SerializeField] public int level;
}
