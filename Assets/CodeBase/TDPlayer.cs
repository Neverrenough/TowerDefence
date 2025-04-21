using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance { get { return Player.Instance as TDPlayer; } }
        private event Action<int> OnGoldUpdate;
        public event Action<int> OnLifeUpdate;
        public event Action<int> OnManaUpdate;
        public UnityEvent ManaUpdate;
        [SerializeField] private Button FireButton;
        [SerializeField] private Button TimeButton;
        public void CheckMana()
        {
            if(Mana < 10)
            {
                Instance.FireButton.interactable = false;
            }
            if(Mana <20)
            {
                Instance.TimeButton.interactable = false;
            }
        }
        public void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.gold);
        }
        public void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Instance.NumLives);
        }
        public void ManaUpdateSubscribe(Action<int> act)
        {
            OnManaUpdate += act;
            act(Instance.Mana);
            ManaUpdate.Invoke();
        }

        [SerializeField] private int gold = 0;
        public void ChangeGold(int change)
        {
            gold += change;
            OnGoldUpdate(gold);
        }
        [SerializeField] public int Mana = 0;
        public void ChangeMana(int change)
        {
            Debug.Log("Changed");
            Mana += change;
            OnManaUpdate(Mana);
            CheckMana();
        }
        public void ReduceLife(int change)
        {
            TakeDamage(change);
            OnLifeUpdate(NumLives);
        }
        [SerializeField] private Tower towerPrefab;
        public void TryBuild(TowerAsset towerAsset, Transform buildSite)
        {
            ChangeGold(-towerAsset.goldCost);
            var tower = Instantiate(towerPrefab, buildSite.position, Quaternion.identity);
            tower.Use(towerAsset);
            Destroy(buildSite.gameObject);
        }
        [SerializeField] private UpgradeAsset healthUpgrade;
        [SerializeField] private UpgradeAsset startGoldUpgrade;
        [SerializeField] private UpgradeAsset FireUpgrade;
        [SerializeField] private UpgradeAsset TimeUpgrade;
        private void Start()
        {
            var level = Upgrades.GetUpgradeLevel(healthUpgrade);
            TakeDamage(-level * 5);
            var level2 = Upgrades.GetUpgradeLevel(startGoldUpgrade);
            gold += level2 * 15;
            if(Upgrades.GetUpgradeLevel(FireUpgrade) == 0)
            {
                FireButton.enabled = false;
                FireButton.interactable = false;
            }
            else
            {
                FireButton.enabled = true;
                FireButton.interactable = true;
            }
            if (Upgrades.GetUpgradeLevel(TimeUpgrade) == 0)
            {
                TimeButton.enabled = false;
                TimeButton.interactable = false;
            }
            else
            {
                TimeButton.enabled = true;
                TimeButton.interactable = true;
            }
        }
    }
}
