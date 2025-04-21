using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefence
{
    [RequireComponent(typeof(Destructible))]
    [RequireComponent(typeof(TDPatrolController))]
    public class Enemy : MonoBehaviour
    {
        public enum ArmorType {Base=0, Mage=1}
        private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions =
        {
            (int power, TDProjectile.DamageType type, int armor) =>
            {
                switch (type)
                {
                    case TDProjectile.DamageType.Magic: return power;
                    default: return Mathf.Max(power - armor,1);
                }
            },
            (int power,TDProjectile.DamageType type, int armor) =>
            {
                if(TDProjectile.DamageType.Base == type)
                    armor = armor / 2;
                return Mathf.Max(power - armor,1);
            }
        };

        [SerializeField] private int damage = 1;
        [SerializeField] private int gold = 1;
        [SerializeField] private int m_armor = 1;
        [SerializeField] private ArmorType m_ArmorType;

        private Destructible m_destructible;

        public event Action OnEnd;
        private void Awake()
        {
            m_destructible = GetComponent<Destructible>();
        }
        private void OnDestroy()
        {
            OnEnd?.Invoke();
        }

        public void Use(EnemyAsset asset)
        {
            var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);

            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animations;

            GetComponent<SpaceShip>().Use(asset);

            GetComponentInChildren<CircleCollider2D>().radius = asset.radius;

            damage = asset.damage;
            m_armor = asset.armor;
            m_ArmorType = asset.armorType;
            gold = asset.gold;
        }
        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(damage);
        }
        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(gold);
        }
        public void TakeDamage(int damage, TDProjectile.DamageType damageType)
        {
            m_destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage,damageType,m_armor));
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
            if (a)
            {
                (target as Enemy).Use(a);
            }
        }
    }
#endif
}
