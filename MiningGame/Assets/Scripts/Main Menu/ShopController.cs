using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public GameObject parentObjectForShop, upGradeUIPrefab;
    [SerializeField]
    public LevelPassSO saveLevelsForNextScene;
    public TextMeshProUGUI moneytext;
    public MainMenuController mainMenuController;
    [SerializeField]
    private List<GameObject> upgradeParents = new List<GameObject>();
    [SerializeField] public List<MasterGearInfo> gearInfo;
    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        moneytext.text = "Money: $" + mainMenuController.money;
        //create and set buttons for shop items
        i = 0;
        foreach(MasterGearInfo gi in gearInfo)
        {
            GameObject tempObj = Instantiate(upGradeUIPrefab, parentObjectForShop.transform);
            tempObj.GetComponent<UpgradeParentUI>().InitUI(gearInfo[i].upName,
                                                           gearInfo[i].level + " --> " + (gearInfo[i].level + 1),
                                                           gearInfo[i].upCost[gearInfo[i].level].ToString(),
                                                           gearInfo[i].upImg,
                                                           CheckCanAfford(gearInfo[i]));
            tempObj.GetComponent<UpgradeParentUI>().upgradeButton.onClick.AddListener(() => BuyUpgrade(tempObj.GetComponent<UpgradeParentUI>()));
            upgradeParents.Add(tempObj);
            i++;
        }        
    }

    public bool CheckCanAfford(MasterGearInfo gInf)
    {
        //level get cost from level
        int amount = gInf.upCost[gInf.level];
        return mainMenuController.money < amount ? false : true;
    }

    public void BuyUpgrade(UpgradeParentUI upgradeObj)
    {
        ///buy upgrade
            //find the cost and subtract the money\\
        MasterGearInfo mg2 = gearInfo.Find((x) => x.upName.Equals(upgradeObj.titleText.text));
        mainMenuController.money -= mg2.upCost[mg2.level];
            //Update UI cost and see if all others are purchaseable
        mg2.level++;
        upgradeObj.lvl2lvlText.text = mg2.level + " --> " + (mg2.level + 1);
        upgradeObj.costText.text = mg2.upCost[mg2.level].ToString();
        //update money UI
        moneytext.text = "Money: $" + mainMenuController.money;
        mainMenuController.moneyText.text = "Money: $" + mainMenuController.money;

        foreach(MasterGearInfo mgInfo in gearInfo)
        {
            foreach(GameObject upUI in upgradeParents)
            {
                if(mgInfo.upName.Equals(upUI.GetComponent<UpgradeParentUI>().titleText.text))
                {
                    upUI.GetComponent<UpgradeParentUI>().Buyable(CheckCanAfford(mgInfo));
                }
            }
        }
        //save
        mainMenuController.SaveGameProgress();
    }

    public void SavelevelsForNext()
    {
        int i = 0;
        foreach(SaveStuffObj ss in saveLevelsForNextScene.saveList)
        {
            ss.upName = gearInfo[i].upName;
            ss.level = gearInfo[i].level;
            i++;
        }
    }
}


[System.Serializable]
public class MasterGearInfo
{
    [SerializeField] public string upName;
    [SerializeField] public int level;
    [SerializeField] public int[] upCost;
    [SerializeField] public float[] upgradeValues;//////
    [SerializeField] public Sprite upImg;

    public void Setlevel(int lvl)
    {
        level = lvl;
    }
}