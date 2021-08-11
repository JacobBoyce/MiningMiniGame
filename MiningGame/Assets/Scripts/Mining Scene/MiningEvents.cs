using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiningEvents : MonoBehaviour
{
    public static MiningEvents current;

    public event Action miningStart;

    void Awake()
    {
        current = this;
    }
    
    public void MiningStart()
    {
        if(miningStart != null)
        {
            miningStart();
        }
    }
}
