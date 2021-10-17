using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUIObj : MonoBehaviour
{
    public Image uivisual;
    public MiningManager manager;
    public float countdown;

    public void Start()
    {
        manager = GameObject.Find("MiningManager").GetComponent<MiningManager>();
    }
    // Update is called once per frame
    void Update()
    {
        uivisual.fillAmount = 1 - (manager.curTime / manager.maxTime);
    }
}
