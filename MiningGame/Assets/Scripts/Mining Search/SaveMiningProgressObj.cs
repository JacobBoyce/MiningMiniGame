using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveMiningProgressObj
{
    [SerializeField]
    public InventoryBag bag;
    public float batteryLevel;
    public Vector3 playerPos;
}
