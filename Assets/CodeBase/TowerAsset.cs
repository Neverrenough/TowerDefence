using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class TowerAsset : ScriptableObject
    {
        public int goldCost = 15;
        public Sprite sprite;
        public Sprite GUISprite;
        public TurretProperties turretProperties;
        [SerializeField] private UpgradeAsset requiredUpgrade;
        [SerializeField] private int requariedUpgradeLevel;
        public bool IsAvailable() => !requiredUpgrade || requariedUpgradeLevel <= Upgrades.GetUpgradeLevel(requiredUpgrade);
        public TowerAsset[] m_UpgradesTo;
    }
}
