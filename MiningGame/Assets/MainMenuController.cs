using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    //save money amount, and what level of gear you have

    public void StartMiningSearch()
    {
        SceneManager.LoadScene("MiningSearch");
    }
}
