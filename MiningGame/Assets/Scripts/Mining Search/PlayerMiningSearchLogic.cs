using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiningSearchLogic : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ore")
        {
            Debug.Log("Mine!");
            GameController.current.MineRockUIEnter();
            GameController.current.currentOre = other.GetComponent<OreDetected>().oreT;
            //display ui button
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ore")
        {
            Debug.Log("No Mine!");
            GameController.current.MineRockUIExit();
            GameController.current.currentOre = OreDetected.OreTypes.NONE;
            //display ui button
        }
    }
}
