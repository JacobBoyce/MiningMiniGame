using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeParentUI : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI titleText, lvl2lvlText, costText;
    [SerializeField]
    public Image upgradeImage;
    [SerializeField]
    public Button upgradeButton;

    public void InitUI(string title, string lvl2lvl, string cost, Sprite upImage, bool buyAble)
    {
        titleText.text = title;
        lvl2lvlText.text = lvl2lvl;
        costText.text = cost;
        upgradeImage.sprite = upImage;
        Buyable(buyAble);
    }

    public void Buyable(bool canBuy)
    {
        upgradeButton.interactable = canBuy;
    }
}
