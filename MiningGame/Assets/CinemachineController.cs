using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera restCam;
    public CinemachineVirtualCamera[] vCamsF = new CinemachineVirtualCamera[3];
    private bool restCamBool = true;
    public int targetNum = 0;
    

    public void SwitchPriority(string str, int camNum)
    {
        if(str.Equals("focus"))
        {
            restCam.Priority = 0;
            vCamsF[camNum].Priority = 1;
        }
        else if(str.Equals("rest"))
        {
            TurnOffFocusedCam();
        }
        restCamBool = !restCamBool;
    }

    private void TurnOffFocusedCam()
    {
        restCam.Priority = 1;
        for(int i = 0; i < vCamsF.Length; i++)
        {
            vCamsF[i].Priority = 0;
        }
    }

}
