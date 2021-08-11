using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int level;
    public int power, focus, curXP, maxXP;

    public void CheckLevelUp()
    {
        if(curXP >= maxXP)
        {
            level++;
            curXP -= maxXP;
            UpdateMaxXp();
            if(curXP <= 0) 
            {
                curXP = 0;
            }
            else
            {
                CheckLevelUp();
            }
        }
    }

    public void SaveStatsForMining()
    {
        PlayerPrefs.SetInt("Plevel", level);
        PlayerPrefs.SetInt("Ppower", power);
        PlayerPrefs.SetInt("Pfocus", focus);
    }

    public void UpdateMaxXp()
    {
        maxXP += maxXP * level;
    }
}
