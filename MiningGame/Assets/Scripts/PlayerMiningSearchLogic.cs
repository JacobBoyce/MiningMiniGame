using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiningSearchLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ore")
        {
            Debug.Log("Mine!");
            GameController.current.MineRockUIEnter();
            //display ui button
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ore")
        {
            Debug.Log("No Mine!");
            GameController.current.MineRockUIExit();
            //display ui button
        }
    }
}
