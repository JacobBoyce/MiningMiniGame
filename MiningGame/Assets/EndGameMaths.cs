using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGameMaths : MonoBehaviour
{
    public TextMeshProUGUI outputStr;
    public void CalcluateSale(string rockStuff)
    {
        outputStr.text += rockStuff;
    }
}
