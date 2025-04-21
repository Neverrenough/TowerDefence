using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Image upgradeIcon;
        private int costNumber = 0;
        [SerializeField] private Text level, costText;
        [SerializeField] private Button buyButton;

        public void Initialise()
        {
            upgradeIcon.sprite = asset.sprite;
            var savedlevel = Upgrades.GetUpgradeLevel(asset); 

            if(savedlevel >= asset.costByLevel.Length)
            {
                level.text = "Max";
                buyButton.interactable = false;
                buyButton.transform.Find("Image (1)").gameObject.SetActive(false);
                buyButton.transform.Find("Text").gameObject.SetActive(false);
                costText.text = "X";
                costNumber = int.MaxValue;
            }
            else
            {
                level.text = (savedlevel + 1).ToString();
                costNumber = asset.costByLevel[savedlevel];
                costText.text = costNumber.ToString();
            }      
        }

        internal void CheckCost(int money)
        {
            buyButton.interactable = money >= costNumber;
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(asset);
            Initialise();
        }
    }
}
