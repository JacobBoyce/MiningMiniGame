using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.Serialization;

public class InventoryBagUI : MonoBehaviour
{
    private InventoryBag bag = new InventoryBag();
    [OdinSerialize] public Image[] rockImages;

    [Header("Hide/Show UI Vars")]
    public GameObject invRockUI;
    public GameObject hideB, showB;

    [Header("Inventory Prefab Objects")]
    public GameObject parentUI;

    public GameObject invRockParentPrefab;
    public GameObject invRockItemPrefab;
    
    private GameObject curRockParentObj;
    private GameObject curRockItemLS;

    public void PopulateInvUI(InventoryBag curInv)
    {
        bag = curInv;

        /// Iterate thru every item in the inv and if it has 0 dont show it
        for(int i = 0; i < bag.inv.Length; i++)
        {
            if(bag.inv[i].amountL > 0 || bag.inv[i].amountS > 0)
            {
                //create UI prefab that is the parent to hold both or one of these values
                curRockParentObj = Instantiate(invRockParentPrefab, parentUI.transform);

                //create LARGE rock image and number
                if(bag.inv[i].amountL > 0)
                {
                    //create UI prefab for a large rock of bag.inv[i].rocktype
                    curRockItemLS = Instantiate(invRockItemPrefab, curRockParentObj.transform);
                    //set image
                    curRockItemLS.GetComponent<RockUI>().rockImage = GetRockImage(bag.inv[i].rockType, true);
                    //set number
                    curRockItemLS.GetComponent<RockUI>().numText.text = bag.inv[i].amountL.ToString();
                }
                //create SMALL rock image and number
                if(bag.inv[i].amountS > 0)
                {
                    //create UI prefab for a small rock of bag.inv[i].rocktype
                    curRockItemLS = Instantiate(invRockItemPrefab, curRockParentObj.transform);
                    //set image
                    curRockItemLS.GetComponent<RockUI>().rockImage = GetRockImage(bag.inv[i].rockType, false);
                    //set number
                    curRockItemLS.GetComponent<RockUI>().numText.text = bag.inv[i].amountS.ToString();
                }
            }
        }
    }

    public Image GetRockImage(inventoryItems.RockType rType, bool isLarge)
    {
        Image rImg;
       
        if(rType == inventoryItems.RockType.COPPER)
        {
            return rImg = isLarge ? rockImages[1] : rockImages[2];
        }
        else if(rType == inventoryItems.RockType.IRON)
        {
            return rImg = isLarge ? rockImages[3] : rockImages[4];
        }
        else if(rType == inventoryItems.RockType.COAL)
        {
            return rImg = isLarge ? rockImages[5] : rockImages[6];
        }
        return rockImages[0];
    }

    public void ShowInvUI()
    {
        //animate ui to slide on screen
        showB.SetActive(false);
        hideB.SetActive(true);
        invRockUI.SetActive(true);
    }

    public void HideInvUI()
    {
        //animate ui to slide off screen
        showB.SetActive(true);
        hideB.SetActive(false);
        invRockUI.SetActive(false);
    }
}
